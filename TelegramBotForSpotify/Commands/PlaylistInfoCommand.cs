using TelegramBotForSpotify.Interfaces;
using TelegramBotForSpotify.Services;

namespace TelegramBotForSpotify.Commands;

public class PlaylistInfoCommand : ICommand
{
    private readonly ISpotifyPlaylistService _spotifyPlaylistService;
    private readonly TelegramService _telegramService;
    private readonly string _userId;

    public PlaylistInfoCommand(ISpotifyPlaylistService spotifyPlaylistService, TelegramService telegramService, string userId)
    {
        _spotifyPlaylistService = spotifyPlaylistService;
        _telegramService = telegramService;
        _userId = userId;
    }

    public async Task Execute()
    {
        var allPlaylists = await _spotifyPlaylistService.GetAllFavoritePlaylists(_userId);
        foreach (var playlistInfo in allPlaylists)
        {
            var message = $"Playlist: {playlistInfo.Name}\nTracks: {playlistInfo.Tracks.Total}";
            await _telegramService.SendMessage(chatId: "your_chat_id", text: message);
            await Task.Delay(1000);
        }
    }
}