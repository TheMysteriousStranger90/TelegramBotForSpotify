using TelegramBotForSpotify.Interfaces;
using TelegramBotForSpotify.Services;

namespace TelegramBotForSpotify.Commands;

public class PlaylistInfoCommand : ICommand
{
    private readonly SpotifyService _spotifyService;
    private readonly TelegramService _telegramService;
    private readonly string _userId;

    public PlaylistInfoCommand(SpotifyService spotifyService, string botToken, string userId)
    {
        _spotifyService = spotifyService;
        _telegramService = TelegramService.Instance(botToken);
        _userId = userId;
    }

    public async void Execute()
    {
        var allPlaylists = await _spotifyService.GetAllFavoritePlaylists(_userId);
        foreach (var playlistInfo in allPlaylists)
        {
            var message = $"Playlist: {playlistInfo.Name}\nTracks: {playlistInfo.Tracks.Total}\nDescription: {playlistInfo.Description}";
            await _telegramService.SendMessage(chatId: "your_chat_id", text: message);
            await Task.Delay(1000);
        }
    }
}