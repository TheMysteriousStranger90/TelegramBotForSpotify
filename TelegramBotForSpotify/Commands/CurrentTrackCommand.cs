using TelegramBotForSpotify.Interfaces;
using TelegramBotForSpotify.Services;

namespace TelegramBotForSpotify.Commands;

public class CurrentTrackCommand : ICommand
{
    private readonly SpotifyService _spotifyService;

    public CurrentTrackCommand(SpotifyService spotifyService)
    {
        _spotifyService = spotifyService;
    }

    public void Execute()
    {
        var track = _spotifyService.GetCurrentTrack();
        // Отправьте информацию о треке в Telegram
    }
}