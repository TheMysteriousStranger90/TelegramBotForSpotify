using SpotifyAPI.Web;
using TelegramBotForSpotify.Services;

namespace TelegramBotForSpotify.Interfaces;

public interface ISpotifyAuthorizationService
{
    Task<string?> Authorize(string code);
    string GetAuthorizationUrl(string state);
    SpotifyClient GetSpotifyClient();
    Task<string> GetTokenAsync();
}