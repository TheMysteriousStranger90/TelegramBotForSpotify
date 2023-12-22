using SpotifyAPI.Web;
using TelegramBotForSpotify.Interfaces;

namespace TelegramBotForSpotify.Services;

public class SpotifyTracksService : ISpotifyTracksService
{
    private readonly ISpotifyClientFactory _spotify;

    public SpotifyTracksService(ISpotifyClientFactory spotify)
    {
        _spotify = spotify;
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
            Console.WriteLine($"Spotify API error: {e.Message}");
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            throw;
        }
    }
}