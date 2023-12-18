using SpotifyAPI.Web;

namespace TelegramBotForSpotify.Interfaces;

public interface ISpotifyClientFactory
{
    Task<SpotifyClient> CreateSpotifyClient();
}