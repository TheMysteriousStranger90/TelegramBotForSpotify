using SpotifyAPI.Web;
using TelegramBotForSpotify.Interfaces;

namespace TelegramBotForSpotify.Services;

public class SpotifyAlbumService : ISpotifyAlbumService
{
    private readonly SpotifyClient _spotify;

    public SpotifyAlbumService(SpotifyClient spotify)
    {
        _spotify = spotify;
    }

    public async Task<List<SavedAlbum>> GetAllFavoriteAlbums()
    {
        try
        {
            var allAlbums = new List<SavedAlbum>();

            await foreach (var album in _spotify.Paginate(await _spotify.Library.GetAlbums()))
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