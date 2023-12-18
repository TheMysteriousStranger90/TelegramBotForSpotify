using System.Net;
using System.Text;
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
        return _spotifyAuthorizationService.GetSpotifyClient();
    }
}