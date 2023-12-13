using TelegramBotForSpotify.Interfaces;
using TelegramBotForSpotify.Services;

namespace TelegramBotForSpotify.Commands;

public class AuthorizeCommand : ICommand
{
    private readonly SpotifyService _spotifyService;

    public AuthorizeCommand(SpotifyService spotifyService)
    {
        _spotifyService = spotifyService;
    }

    public void Execute()
    {
        _spotifyService.Authorize();
    }
}