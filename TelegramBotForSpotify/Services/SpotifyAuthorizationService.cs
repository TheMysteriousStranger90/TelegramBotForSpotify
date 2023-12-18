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
            await SaveRefreshToken(_refreshToken);

            return responseObject.Value<string>("access_token");
        }
        else
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception(
                $"Failed to authorize with Spotify. Status code: {response.StatusCode}, Response: {errorContent}");
        }
    }

    public async Task<string> GetTokenAsync()
    {
        if (_refreshToken == null)
        {
            await LoadRefreshToken();
        }

        if (_refreshToken == null)
        {
            throw new InvalidOperationException("Refresh token is not available. Please authorize first.");
        }

        var config = SpotifyClientConfig.CreateDefault();
        var request = new AuthorizationCodeRefreshRequest(_spotifySettings.ClientId, _spotifySettings.ClientSecret, _refreshToken);
        var response = await new OAuthClient(config).RequestToken(request);

        if (string.IsNullOrEmpty(response.AccessToken))
        {
            throw new Exception("Failed to refresh token: Access token is null or empty");
        }

        _refreshToken = response.RefreshToken;
        await SaveRefreshToken(_refreshToken);

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
            ["scope"] = "user-read-private user-read-email user-library-read user-read-currently-playing user-read-playback-state"
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
            
            if (responseObject.ContainsKey("refresh_token"))
            {
                _refreshToken = responseObject.Value<string>("refresh_token");
                await SaveRefreshToken(_refreshToken);
            }

            return responseObject.Value<string>("access_token");
        }
        else
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception(
                $"Failed to refresh Spotify access token. Status code: {response.StatusCode}, Response: {errorContent}");
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
    
    public async Task SaveRefreshToken(string refreshToken)
    {
        var json = JsonConvert.SerializeObject(refreshToken);
        await File.WriteAllTextAsync(Path.Combine("wwwroot", "data", "refresh.json"), json);
    }

    public async Task LoadRefreshToken()
    {
        var filePath = Path.Combine("wwwroot", "data", "refresh.json");
        if (File.Exists(filePath))
        {
            var json = await File.ReadAllTextAsync(filePath);
            _refreshToken = JsonConvert.DeserializeObject<string>(json);
        }
    }
    
    public bool IsUserAuthorized()
    {
        return !string.IsNullOrEmpty(_refreshToken);
    }
}