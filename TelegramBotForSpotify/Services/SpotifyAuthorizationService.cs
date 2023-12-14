using Microsoft.Extensions.Options;
using SpotifyAPI.Web;
using TelegramBotForSpotify.Auth;
using TelegramBotForSpotify.Interfaces;

namespace TelegramBotForSpotify.Services;

public class SpotifyAuthorizationService : ISpotifyAuthorizationService
{
    private readonly SpotifySettings _spotifySettings;
    private SpotifyClient _spotify;
    public SpotifyAuthorizationService(IOptions<SpotifySettings> spotifySettings)
    {
        _spotifySettings = spotifySettings.Value;
    }

    public async Task Authorize()
    {
        var config = SpotifyClientConfig.CreateDefault();
        var request = new ClientCredentialsRequest(_spotifySettings.ClientId, _spotifySettings.ClientSecret);
        var response = await new OAuthClient(config).RequestToken(request);

        _spotify = new SpotifyClient(config.WithToken(response.AccessToken));
    }

    public string GetAuthorizationUrl(string state)
    {
        var request = new LoginRequest(new Uri(_spotifySettings.RedirectUri), _spotifySettings.ClientId, LoginRequest.ResponseType.Code)
        {
            Scope = new List<string> { Scopes.UserReadPrivate, Scopes.UserReadEmail, Scopes.UserLibraryRead },
            State = state
        };

        return request.ToUri().ToString();
    }

    public SpotifyClient GetSpotifyClient()
    {
        return _spotify;
    }
}