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
    private string _tokenFilePath = Path.Combine("wwwroot", "data", "mytoken.json");

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

            var tokenResponse = new AuthorizationCodeTokenResponse
            {
                AccessToken = responseObject.Value<string>("access_token"),
                RefreshToken = responseObject.Value<string>("refresh_token"),
                ExpiresIn = responseObject.Value<int>("expires_in"),
                TokenType = "Bearer",
                Scope =
                    "user-read-private user-read-email user-library-read user-read-currently-playing user-read-playback-state playlist-read-private"
            };

            _refreshToken = tokenResponse.RefreshToken;

            _tokenFilePath = GenerateTokenFilePath();
            await SaveTokenToFile(tokenResponse);

            return tokenResponse.AccessToken;
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
        var request =
            new AuthorizationCodeRefreshRequest(_spotifySettings.ClientId, _spotifySettings.ClientSecret,
                _refreshToken);
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
            ["scope"] =
                "user-read-private user-read-email user-library-read user-read-currently-playing user-read-playback-state playlist-read-private"
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

            var tokenResponse = new AuthorizationCodeTokenResponse
            {
                AccessToken = responseObject.Value<string>("access_token"),
                RefreshToken = responseObject.ContainsKey("refresh_token")
                    ? responseObject.Value<string>("refresh_token")
                    : refreshToken,
                ExpiresIn = responseObject.Value<int>("expires_in"),
                TokenType = "Bearer",
                Scope =
                    "user-read-private user-read-email user-library-read user-read-currently-playing user-read-playback-state playlist-read-private"
            };


            _refreshToken = tokenResponse.RefreshToken;

            await SaveTokenToFile(tokenResponse);

            return tokenResponse.AccessToken;
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


    public bool IsUserAuthorized()
    {
        return !string.IsNullOrEmpty(_refreshToken);
    }

    public void InitializeSpotifyClient()
    {
        var tokenResponse = LoadTokenFromFile();

        var config = SpotifyClientConfig
            .CreateDefault()
            .WithAuthenticator(new AuthorizationCodeAuthenticator(
                _spotifySettings.ClientId,
                _spotifySettings.ClientSecret,
                tokenResponse
            ));

        _spotify = new SpotifyClient(config);
    }

    public async Task<SpotifyClient> GetSpotifyClientAsync()
    {
        if (IsAccessTokenExpired())
        {
            var refreshToken = GetRefreshToken();
            var newAccessToken = await RefreshToken(refreshToken);
            InitializeSpotifyClient(newAccessToken);
        }

        return _spotify;
    }

    public async Task<string> GetUserTokenAsync()
    {
        var tokenResponse = LoadTokenFromFile();

        if (tokenResponse == null)
        {
            throw new Exception("No token found. Please authorize first.");
        }

        if (IsAccessTokenExpired())
        {
            var newAccessToken = await RefreshToken(tokenResponse.RefreshToken);
            tokenResponse.AccessToken = newAccessToken;
            await SaveTokenToFile(tokenResponse);
        }

        return tokenResponse.AccessToken;
    }

    public async Task SaveRefreshToken(string refreshToken)
    {
        var json = JsonConvert.SerializeObject(refreshToken);
        await File.WriteAllTextAsync(_tokenFilePath, json);
    }

    public async Task LoadRefreshToken()
    {
        if (File.Exists(_tokenFilePath))
        {
            var json = await File.ReadAllTextAsync(_tokenFilePath);
            _refreshToken = JsonConvert.DeserializeObject<string>(json);
        }
    }

    public async Task SaveTokenToFile(AuthorizationCodeTokenResponse tokenResponse)
    {
        var tokenData = JsonConvert.SerializeObject(tokenResponse);
        var directoryPath = Path.GetDirectoryName(_tokenFilePath);

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        File.WriteAllText(_tokenFilePath, tokenData);
    }

    public AuthorizationCodeTokenResponse LoadTokenFromFile()
    {
        if (File.Exists(_tokenFilePath))
        {
            var json = File.ReadAllText(_tokenFilePath);
            return JsonConvert.DeserializeObject<AuthorizationCodeTokenResponse>(json);
        }

        return null;
    }

    private string GenerateTokenFilePath()
    {
        var random = new Random();
        var randomNumber = random.Next(10000, 99999);
        return Path.Combine("wwwroot", "data", $"mytoken_{randomNumber}.json");
    }
}