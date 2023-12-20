using SpotifyAPI.Web;
using TelegramBotForSpotify.Interfaces;

namespace TelegramBotForSpotify.Services;

public class SpotifyTrackService : ISpotifyTrackService
{
    private readonly ISpotifyClientFactory _spotify;

    public SpotifyTrackService(ISpotifyClientFactory spotify)
    {
        _spotify = spotify;
    }

    public async Task<FullTrack> GetCurrentTrack()
    {
        try
        {
            var spotifyClient = await _spotify.CreateSpotifyClient();
            var playback = await spotifyClient.Player.GetCurrentPlayback();
            if (playback?.Item is FullTrack track)
            {
                return track;
            }
            else
            {
                return null;
            }
        }
        catch (APIException e)
        {
            Console.WriteLine(e.ToString());
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            throw;
        }
    }
}