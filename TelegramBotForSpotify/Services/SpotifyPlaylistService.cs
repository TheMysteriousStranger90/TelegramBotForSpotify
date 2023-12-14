using SpotifyAPI.Web;
using TelegramBotForSpotify.Interfaces;

namespace TelegramBotForSpotify.Services;

public class SpotifyPlaylistService : ISpotifyPlaylistService
{
    private readonly SpotifyClient _spotify;

    public SpotifyPlaylistService(SpotifyClient spotify)
    {
        _spotify = spotify;
    }

    public async Task<List<FullPlaylist>> GetAllFavoritePlaylists(string userId)
    {
        try
        {
            var allPlaylists = new List<FullPlaylist>();

            await foreach (var playlist in _spotify.Paginate(await _spotify.Playlists.GetUsers(userId)))
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