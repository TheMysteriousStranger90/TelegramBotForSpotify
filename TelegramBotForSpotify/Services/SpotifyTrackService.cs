using SpotifyAPI.Web;
using TelegramBotForSpotify.Interfaces;

namespace TelegramBotForSpotify.Services;

public class SpotifyTrackService : ISpotifyTrackService
{
    private readonly SpotifyClient _spotify;

    public SpotifyTrackService(SpotifyClient spotify)
    {
        _spotify = spotify;
    }

    public async Task<FullTrack> GetCurrentTrack()
    {
        try
        {
            var playback = await _spotify.Player.GetCurrentPlayback();
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

    public async Task<List<SavedTrack>> GetAllFavoriteTracks()
    {
        try
        {
            var allTracks = new List<SavedTrack>();

            await foreach (var track in _spotify.Paginate(await _spotify.Library.GetTracks()))
            {
                allTracks.Add(track);
            }

            return allTracks;
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