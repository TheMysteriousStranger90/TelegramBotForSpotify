using System.Text.RegularExpressions;
using MediaToolkit;
using MediaToolkit.Model;
using VideoLibrary;

namespace TelegramBotForSpotify.Services;

public class YouTubeDownloadService
{
    private readonly string _tempPath;
    private readonly int _maxRetries = 3;

    public YouTubeDownloadService()
    {
        _tempPath = Path.Combine(Path.GetTempPath(), "SpotifyBotDownloads");
        Directory.CreateDirectory(_tempPath);
    }

    public async Task<string> DownloadAudioAsync(string trackName, string artist, int retryCount = 0)
    {
        try
        {
            var searchQueries = new[]
            {
                $"{trackName} {artist} audio",
                $"{trackName} {artist} official",
                $"{trackName} {artist}"
            };

            foreach (var query in searchQueries)
            {
                var video = await GetVideo(query);
                if (video != null)
                {
                    var outputPath = Path.Combine(_tempPath, $"{SafeFileName(trackName)} - {SafeFileName(artist)}.mp3");
                    await DownloadAndConvertVideo(video, outputPath);
                    return outputPath;
                }
            }

            Console.WriteLine($"No videos found for {trackName} - {artist}");
            return null;
        }
        catch (Exception ex) when (ex.Message.Contains("bot") && retryCount < _maxRetries)
        {
            Console.WriteLine(
                $"Rate limit detected, retrying in 5 seconds... (Attempt {retryCount + 1}/{_maxRetries})");
            await Task.Delay(5000);
            return await DownloadAudioAsync(trackName, artist, retryCount + 1);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Download error: {ex.Message}");
            return null;
        }
    }

    private async Task<Video> GetVideo(string query)
    {
        var youtube = YouTube.Default;
        var searchUrl = $"https://www.youtube.com/results?search_query={Uri.EscapeDataString(query)}";

        using (var client = new HttpClient())
        {
            var html = await client.GetStringAsync(searchUrl);
            var videoIdMatch = Regex.Match(html, @"videoId"":""([^""]+)");

            if (!videoIdMatch.Success)
                return null;

            var videoUrl = $"https://www.youtube.com/watch?v={videoIdMatch.Groups[1].Value}";
            return youtube.GetAllVideos(videoUrl)
                .OrderByDescending(v => v.AudioBitrate)
                .FirstOrDefault();
        }
    }

    private async Task DownloadAndConvertVideo(Video video, string outputPath)
    {
        var tempPath = Path.Combine(_tempPath, $"{Guid.NewGuid()}{video.FileExtension}");

        try
        {
            File.WriteAllBytes(tempPath, video.GetBytes());
            using (var engine = new Engine())
            {
                engine.Convert(new MediaFile { Filename = tempPath },
                    new MediaFile { Filename = outputPath });
            }
        }
        finally
        {
            if (File.Exists(tempPath))
                File.Delete(tempPath);
        }
    }

    private string SafeFileName(string fileName)
    {
        var invalid = Path.GetInvalidFileNameChars();
        return string.Join("_", fileName.Split(invalid, StringSplitOptions.RemoveEmptyEntries));
    }
}