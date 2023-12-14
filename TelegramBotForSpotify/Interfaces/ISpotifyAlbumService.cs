using SpotifyAPI.Web;

namespace TelegramBotForSpotify.Interfaces;

public interface ISpotifyAlbumService
{
    Task<List<SavedAlbum>> GetAllFavoriteAlbums();
}