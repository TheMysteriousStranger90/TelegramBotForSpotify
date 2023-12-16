using SpotifyAPI.Web;
using TelegramBotForSpotify.Interfaces;

namespace TelegramBotForSpotify.Services;

public class SpotifyAlbumService : ISpotifyAlbumService
{
    private readonly ISpotifyClientFactory _spotify;

    public SpotifyAlbumService(ISpotifyClientFactory spotify)
    {
        _spotify = spotify;
    }

    public async Task<List<SavedAlbum>> GetAllFavoriteAlbums()
    {
        try
        {
            var spotifyClient = _spotify.CreateSpotifyClient();
            var allAlbums = new List<SavedAlbum>();

            await foreach (var album in spotifyClient.Paginate(await spotifyClient.Library.GetAlbums()))
            {
                allAlbums.Add(album);
            }

            return allAlbums;
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