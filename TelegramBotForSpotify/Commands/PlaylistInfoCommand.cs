using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
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

    public async Task ExecuteAsync(Update update)
    {
        var message = update.Message;
        if (message != null && message.Type == MessageType.Text)
        {
            var allPlaylists = await _spotifyPlaylistService.GetAllFavoritePlaylists();
            foreach (var playlistInfo in allPlaylists)
            {
                var _message = $"Playlist: {playlistInfo.Name}\nTracks: {playlistInfo.Tracks.Total}";
                await _telegramService.SendMessage(message.Chat.Id.ToString(), text: _message);
                await Task.Delay(1000);
            }
        }
        else
        {
            var _message = "No favorite playlists.";
            await _telegramService.SendMessage(message.Chat.Id.ToString(), text: _message);
        }
    }
}