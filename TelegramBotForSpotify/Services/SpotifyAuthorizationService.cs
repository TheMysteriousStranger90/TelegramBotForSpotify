using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SpotifyAPI.Web;
using TelegramBotForSpotify.Auth;
using TelegramBotForSpotify.Interfaces;

namespace TelegramBotForSpotify.Services;

public class SpotifyAuthorizationService : ISpotifyAuthorizationService
{
    private readonly SpotifySettings _spotifySettings;
    private string _refreshToken;
    private SpotifyClient _spotify;
    private DateTime _tokenExpirationTime;
    private readonly HttpClient _httpClient;

    public SpotifyAuthorizationService(IOptions<SpotifySettings> spotifySettings)
    {
        _spotifySettings = spotifySettings.Value;
        _httpClient = new HttpClient();
    }

    public async Task<string> Authorize(string code)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "https://accounts.spotify.com/api/token");

        request.Headers.Authorization = new AuthenticationHeaderValue(
            "Basic",
            Convert.ToBase64String(
                Encoding.UTF8.GetBytes($"{_spotifySettings.ClientId}:{_spotifySettings.ClientSecret}")));

        request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["grant_type"] = "authorization_code",
            ["code"] = code,
            ["redirect_uri"] = _spotifySettings.RedirectUri
        });

        var response = await _httpClient.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<JObject>(responseContent);

            _tokenExpirationTime = DateTime.UtcNow.AddSeconds(responseObject.Value<int>("expires_in"));
            _refreshToken = responseObject.Value<string>("refresh_token");

            return responseObject.Value<string>("access_token");
        }
        else
        {
            throw new Exception("Failed to authorize with Spotify");
        }
    }

    public async Task<string> GetTokenAsync()
    {
        var config = SpotifyClientConfig.CreateDefault();
        var request = new ClientCredentialsRequest(_spotifySettings.ClientId, _spotifySettings.ClientSecret);
        var response = await new OAuthClient(config).RequestToken(request);

        if (response.IsExpired)
        {
            throw new Exception("Failed to get token from Spotify");
        }

        return response.AccessToken;
    }

    public string GetAuthorizationUrl(string state)
    {
        var queryParams = new Dictionary<string, string>
        {
            ["client_id"] = _spotifySettings.ClientId,
            ["response_type"] = "code",
            ["redirect_uri"] = _spotifySettings.RedirectUri,
            ["state"] = state,
            ["scope"] = "user-read-private user-read-email user-library-read"
        };

        var queryString = string.Join("&", queryParams.Select(kvp => $"{kvp.Key}={Uri.EscapeDataString(kvp.Value)}"));

        return $"https://accounts.spotify.com/authorize?{queryString}";
    }

    public async Task<string> RefreshToken(string refreshToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "https://accounts.spotify.com/api/token");

        request.Headers.Authorization = new AuthenticationHeaderValue(
            "Basic",
            Convert.ToBase64String(
                Encoding.UTF8.GetBytes($"{_spotifySettings.ClientId}:{_spotifySettings.ClientSecret}")));

        request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["grant_type"] = "refresh_token",
            ["refresh_token"] = refreshToken
        });

        var response = await _httpClient.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<JObject>(responseContent);

            _tokenExpirationTime = DateTime.UtcNow.AddSeconds(responseObject.Value<int>("expires_in"));

            return responseObject.Value<string>("access_token");
        }
        else
        {
            throw new Exception("Failed to refresh Spotify access token");
        }
    }

    public bool IsAccessTokenExpired()
    {
        if (_tokenExpirationTime == DateTime.MinValue)
        {
            return true;
        }

        TimeSpan difference = _tokenExpirationTime - DateTime.UtcNow;

        if (difference.TotalSeconds <= 0)
        {
            return true;
        }

        return false;
    }

    public string GetRefreshToken()
    {
        return _refreshToken;
    }

    public void InitializeSpotifyClient(string token)
    {
        _spotify = new SpotifyClient(token);
    }

    public SpotifyClient GetSpotifyClient()
    {
        return _spotify;
    }
}