using System.Net;
using System.Text;
using SpotifyAPI.Web;
using TelegramBotForSpotify.Interfaces;

namespace TelegramBotForSpotify;

public class SpotifyClientFactory : ISpotifyClientFactory
{
    private readonly ISpotifyAuthorizationService _spotifyAuthorizationService;
    private readonly IConfiguration _configuration;

    public SpotifyClientFactory(ISpotifyAuthorizationService spotifyAuthorizationService, IConfiguration configuration)
    {
        _spotifyAuthorizationService = spotifyAuthorizationService;
        _configuration = configuration;
    }

    public async Task<SpotifyClient> CreateSpotifyClientAsync()
    {
        var token = await _spotifyAuthorizationService.GetUserTokenAsync();

        if (string.IsNullOrEmpty(token))
        {
            throw new Exception("Failed to get user token");
        }

        var config = SpotifyClientConfig.CreateDefault().WithToken(token);
        return new SpotifyClient(config);
    }

    public async Task<string> CreateUserTokenAsync()
    {
        return await _spotifyAuthorizationService.GetUserTokenAsync();
    }
}