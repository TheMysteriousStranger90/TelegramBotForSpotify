using SpotifyAPI.Web;

namespace TelegramBotForSpotify.Interfaces;

public interface ISpotifyTracksService
{
    Task<List<SavedTrack>> GetAllFavoriteTracks();
}