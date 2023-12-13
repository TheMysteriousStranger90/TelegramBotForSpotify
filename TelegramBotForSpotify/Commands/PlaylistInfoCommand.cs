using TelegramBotForSpotify.Interfaces;
using TelegramBotForSpotify.Services;

namespace TelegramBotForSpotify.Commands;

// Класс для команды отображения информации о плейлисте
public class PlaylistInfoCommand : ICommand
{
    private readonly SpotifyService _spotifyService;
    private readonly string _playlistId;

    public PlaylistInfoCommand(SpotifyService spotifyService, string playlistId)
    {
        _spotifyService = spotifyService;
        _playlistId = playlistId;
    }

    public void Execute()
    {
        var playlistInfo = _spotifyService.GetPlaylistInfo(_playlistId);
        // Отправьте информацию о плейлисте в Telegram
    }
}