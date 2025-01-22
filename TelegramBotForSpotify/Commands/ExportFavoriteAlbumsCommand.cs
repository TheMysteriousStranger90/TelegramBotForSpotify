using System.Xml.Serialization;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBotForSpotify.Interfaces;
using TelegramBotForSpotify.Models;
using File = System.IO.File;

namespace TelegramBotForSpotify.Commands;

public class ExportFavoriteAlbumsCommand : ICommand
{
    private readonly ISpotifyAlbumService _spotifyAlbumService;
    private readonly ITelegramService _telegramService;

    public ExportFavoriteAlbumsCommand(ISpotifyAlbumService spotifyAlbumService, ITelegramService telegramService)
    {
        _spotifyAlbumService = spotifyAlbumService;
        _telegramService = telegramService;
    }

    public async Task ExecuteAsync(Update update)
    {
        var message = update.Message;
        if (message == null || message.Type != MessageType.Text)
            return;

        await _telegramService.SendMessage(message.Chat.Id.ToString(), "Preparing your favorite albums export...");

        var albums = await _spotifyAlbumService.GetAllFavoriteAlbums();
        var albumsData = albums.Select(a => new AlbumData
        {
            Name = a.Album.Name,
            Artist = a.Album.Artists[0].Name,
            TracksCount = a.Album.Tracks.Items.Count,
            ReleaseDate = a.Album.ReleaseDate
        }).ToList();

        var xmlFilePath = Path.Combine(Path.GetTempPath(), $"favorite_albums_{DateTime.Now:yyyyMMddHHmmss}.xml");

        var serializer = new XmlSerializer(typeof(List<AlbumData>));
        using (var writer = new StreamWriter(xmlFilePath))
        {
            serializer.Serialize(writer, albumsData);
        }

        await _telegramService.SendDocumentAsync(
            message.Chat.Id.ToString(),
            xmlFilePath,
            "Here are your favorite albums in XML format"
        );

        File.Delete(xmlFilePath);
    }
}