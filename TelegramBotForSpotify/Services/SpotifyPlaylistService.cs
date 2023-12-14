using SpotifyAPI.Web;
using TelegramBotForSpotify.Interfaces;

namespace TelegramBotForSpotify.Services;

public class SpotifyPlaylistService : ISpotifyPlaylistService
{
    private readonly ISpotifyClientFactory _spotify;

    public SpotifyPlaylistService(ISpotifyClientFactory spotify)
    {
        _spotify = spotify;
    }

    public async Task<List<FullPlaylist>> GetAllFavoritePlaylists(string userId)
    {
        try
        {
            var spotifyClient = await _spotify.CreateSpotifyClientAsync();

            var allPlaylists = new List<FullPlaylist>();

            await foreach (var playlist in spotifyClient.Paginate(await spotifyClient.Playlists.GetUsers(userId)))
            {
                allPlaylists.Add(playlist);
            }

            return allPlaylists;
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