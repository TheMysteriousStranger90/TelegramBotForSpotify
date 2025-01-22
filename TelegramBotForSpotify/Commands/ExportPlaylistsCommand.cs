using System.Xml.Serialization;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBotForSpotify.Interfaces;
using TelegramBotForSpotify.Models;
using File = System.IO.File;

namespace TelegramBotForSpotify.Commands;

public class ExportPlaylistsCommand : ICommand
{
    private readonly ISpotifyPlaylistService _spotifyPlaylistService;
    private readonly ITelegramService _telegramService;

    public ExportPlaylistsCommand(ISpotifyPlaylistService spotifyPlaylistService, ITelegramService telegramService)
    {
        _spotifyPlaylistService = spotifyPlaylistService;
        _telegramService = telegramService;
    }

    public async Task ExecuteAsync(Update update)
    {
        var message = update.Message;
        if (message == null || message.Type != MessageType.Text)
            return;

        await _telegramService.SendMessage(message.Chat.Id.ToString(), "Preparing your playlists export...");

        var playlists = await _spotifyPlaylistService.GetAllFavoritePlaylists();
        var playlistsData = playlists.Select(p => new PlaylistData
        {
            Name = p.Name,
            Description = p.Description,
            TracksCount = p.Tracks.Total,
            IsPublic = p.Public ?? false,
            Owner = p.Owner.DisplayName,
            SpotifyUrl = p.ExternalUrls["spotify"],
            ImageUrl = p.Images.FirstOrDefault()?.Url }).ToList();

        var xmlFilePath = Path.Combine(Path.GetTempPath(), $"playlists_{DateTime.Now:yyyyMMddHHmmss}.xml");
        
        var serializer = new XmlSerializer(typeof(List<PlaylistData>));
        using (var writer = new StreamWriter(xmlFilePath))
        {
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);
            serializer.Serialize(writer, playlistsData, namespaces);
        }

        await _telegramService.SendDocumentAsync(
            message.Chat.Id.ToString(), 
            xmlFilePath, 
            $"Your playlists ({playlists.Count} playlists)"
        );

        File.Delete(xmlFilePath);
    }
}