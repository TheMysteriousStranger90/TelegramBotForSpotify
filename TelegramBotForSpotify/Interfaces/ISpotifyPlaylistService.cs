using SpotifyAPI.Web;

namespace TelegramBotForSpotify.Interfaces;

public interface ISpotifyPlaylistService
{
    Task<List<FullPlaylist>> GetAllFavoritePlaylists(string userId);
}