using SpotifyAPI.Web;
using TelegramBotForSpotify.Interfaces;

namespace TelegramBotForSpotify;

public class SpotifyClientFactory : ISpotifyClientFactory
{
    private readonly ISpotifyAuthorizationService _spotifyAuthorizationService;

    public SpotifyClientFactory(ISpotifyAuthorizationService spotifyAuthorizationService)
    {
        _spotifyAuthorizationService = spotifyAuthorizationService;
    }

    public async Task<SpotifyClient> CreateSpotifyClient()
    {
        if (_spotifyAuthorizationService.IsAccessTokenExpired())
        {
            var refreshToken = _spotifyAuthorizationService.GetRefreshToken();
            var newAccessToken = await _spotifyAuthorizationService.RefreshToken(refreshToken);

            _spotifyAuthorizationService.InitializeSpotifyClient(newAccessToken);
        }

        return _spotifyAuthorizationService.GetSpotifyClient();
    }
}