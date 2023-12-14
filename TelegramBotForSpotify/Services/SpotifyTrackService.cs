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
            var spotifyClient = await _spotify.CreateSpotifyClientAsync();
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

    public async Task<List<SavedTrack>> GetAllFavoriteTracks()
    {
        try
        {
            var spotifyClient = await _spotify.CreateSpotifyClientAsync();
            var allTracks = new List<SavedTrack>();

            await foreach (var track in spotifyClient.Paginate(await spotifyClient.Library.GetTracks()))
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