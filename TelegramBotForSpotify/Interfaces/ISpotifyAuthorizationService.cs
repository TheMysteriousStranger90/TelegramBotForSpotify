using SpotifyAPI.Web;
using TelegramBotForSpotify.Services;

namespace TelegramBotForSpotify.Interfaces;

public interface ISpotifyAuthorizationService
{
    Task<string?> Authorize(string code);
    Task<string> GetTokenAsync();
    Task<string> RefreshToken(string refreshToken);
    SpotifyClient GetSpotifyClient();
    string GetAuthorizationUrl(string state);
    public bool IsAccessTokenExpired();
    public string GetRefreshToken();
    void InitializeSpotifyClient(string token);
    Task LoadRefreshToken();
    Task SaveRefreshToken(string refreshToken);
    bool IsUserAuthorized();
}