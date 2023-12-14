using SpotifyAPI.Web;

namespace TelegramBotForSpotify.Interfaces;

public interface ISpotifyAuthorizationService
{
    Task Authorize();
    string GetAuthorizationUrl(string state);
    SpotifyClient GetSpotifyClient();
    Task<string> GetTokenAsync();
}