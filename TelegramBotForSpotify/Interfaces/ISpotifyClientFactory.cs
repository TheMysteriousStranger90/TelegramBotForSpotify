using SpotifyAPI.Web;

namespace TelegramBotForSpotify.Interfaces;

public interface ISpotifyClientFactory
{
    SpotifyClient CreateSpotifyClient();
}