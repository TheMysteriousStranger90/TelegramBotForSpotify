using TelegramBotForSpotify.Interfaces;
using TelegramBotForSpotify.Services;

namespace TelegramBotForSpotify.Commands;

public class PlaylistInfoCommand : ICommand
{
    private readonly ISpotifyPlaylistService _spotifyPlaylistService;
    private readonly ITelegramService _telegramService;

    public PlaylistInfoCommand(ISpotifyPlaylistService spotifyPlaylistService, ITelegramService telegramService)
    {
        _spotifyPlaylistService = spotifyPlaylistService;
        _telegramService = telegramService;
    }

    public async Task Execute()
    {
        var allPlaylists = await _spotifyPlaylistService.GetAllFavoritePlaylists();
        foreach (var playlistInfo in allPlaylists)
        {
            var message = $"Playlist: {playlistInfo.Name}\nTracks: {playlistInfo.Tracks.Total}";
            await _telegramService.SendMessage(chatId: "your_chat_id", text: message);
            await Task.Delay(1000);
        }
    }
}