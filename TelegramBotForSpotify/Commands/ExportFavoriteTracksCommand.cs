using System.Xml.Serialization;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBotForSpotify.Interfaces;
using TelegramBotForSpotify.Models;
using File = System.IO.File;

namespace TelegramBotForSpotify.Commands;

public class ExportFavoriteTracksCommand : ICommand
{
    private readonly ISpotifyTracksService _spotifyTracksService;
    private readonly ITelegramService _telegramService;

    public ExportFavoriteTracksCommand(ISpotifyTracksService spotifyTracksService, ITelegramService telegramService)
    {
        _spotifyTracksService = spotifyTracksService;
        _telegramService = telegramService;
    }

    public async Task ExecuteAsync(Update update)
    {
        var message = update.Message;
        if (message == null || message.Type != MessageType.Text)
            return;

        await _telegramService.SendMessage(message.Chat.Id.ToString(), "Preparing your favorite tracks export...");

        var tracks = await _spotifyTracksService.GetAllFavoriteTracks();
        var tracksData = tracks.Select(t => new TrackData
        {
            Name = t.Track.Name,
            Artist = string.Join(", ", t.Track.Artists.Select(a => a.Name)),
            Album = t.Track.Album.Name,
            Duration = TimeSpan.FromMilliseconds(t.Track.DurationMs).ToString(@"mm\:ss"),
            IsExplicit = t.Track.Explicit,
            SpotifyUrl = t.Track.ExternalUrls["spotify"]
        }).ToList();

        var xmlFilePath = Path.Combine(Path.GetTempPath(), $"favorite_tracks_{DateTime.Now:yyyyMMddHHmmss}.xml");
        
        var serializer = new XmlSerializer(typeof(List<TrackData>));
        using (var writer = new StreamWriter(xmlFilePath))
        {
            serializer.Serialize(writer, tracksData);
        }

        await _telegramService.SendDocumentAsync(
            message.Chat.Id.ToString(), 
            xmlFilePath, 
            $"Your favorite tracks list ({tracks.Count} tracks)"
        );

        File.Delete(xmlFilePath);
    }
}