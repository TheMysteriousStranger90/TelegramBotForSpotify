using Telegram.Bot.Types;
using TelegramBotForSpotify.Interfaces;
using TelegramBotForSpotify.Services;
using File = System.IO.File;

namespace TelegramBotForSpotify.Commands;

public class DownloadCurrentTrackCommand : ICommand
{
    private readonly ISpotifyTrackService _spotifyTrackService;
    private readonly ITelegramService _telegramService;
    private readonly YouTubeDownloadService _youtubeService;

    public DownloadCurrentTrackCommand(
        ISpotifyTrackService spotifyTrackService,
        ITelegramService telegramService)
    {
        _spotifyTrackService = spotifyTrackService;
        _telegramService = telegramService;
        _youtubeService = new YouTubeDownloadService();
    }

    public async Task ExecuteAsync(Update update)
    {
        var message = update.Message;
        if (message == null) return;

        var track = await _spotifyTrackService.GetCurrentTrack();
        if (track == null)
        {
            await _telegramService.SendMessage(message.Chat.Id.ToString(), "No track is currently playing.");
            return;
        }

        await _telegramService.SendMessage(message.Chat.Id.ToString(), "🎵 Searching and downloading the track...");

        var audioPath = await _youtubeService.DownloadAudioAsync(track.Name, track.Artists[0].Name);

        if (audioPath == null)
        {
            await _telegramService.SendMessage(message.Chat.Id.ToString(), "❌ Sorry, couldn't find or download this track.");
            return;
        }

        try
        {
            await _telegramService.SendAudioAsync(
                message.Chat.Id.ToString(),
                audioPath,
                $"🎵 {track.Name} - {track.Artists[0].Name}"
            );
        }
        finally
        {
            if (File.Exists(audioPath))
                File.Delete(audioPath);
        }
    }
}