using SpotifyAPI.Web;

namespace TelegramBotForSpotify.Interfaces;

public interface ISpotifyTrackService
{
    Task<FullTrack> GetCurrentTrack();
}