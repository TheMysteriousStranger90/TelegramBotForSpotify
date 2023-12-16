using SpotifyAPI.Web;
using TelegramBotForSpotify.Interfaces;

namespace TelegramBotForSpotify.Services;

public class SpotifyClientFactory : ISpotifyClientFactory
{
    private readonly ISpotifyAuthorizationService _spotifyAuthorizationService;

    public SpotifyClientFactory(ISpotifyAuthorizationService spotifyAuthorizationService)
    {
        _spotifyAuthorizationService = spotifyAuthorizationService;
    }

    public SpotifyClient CreateSpotifyClient()
    {
        return _spotifyAuthorizationService.GetSpotifyClient();
    }
}