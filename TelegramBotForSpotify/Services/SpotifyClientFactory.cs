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

    public async Task<SpotifyClient> CreateSpotifyClientAsync()
    {
        // Use the authorization service to get the necessary data for SpotifyClient
        var token = await _spotifyAuthorizationService.GetTokenAsync();
        return new SpotifyClient(token);
    }
}