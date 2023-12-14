using SpotifyAPI.Web;

namespace TelegramBotForSpotify.Services;

public class SpotifyService
{
    private SpotifyClient _spotify;

    public async Task Authorize(string clientId, string clientSecret)
    {
        var config = SpotifyClientConfig.CreateDefault();
        var request = new ClientCredentialsRequest(clientId, clientSecret);
        var response = await new OAuthClient(config).RequestToken(request);

        _spotify = new SpotifyClient(config.WithToken(response.AccessToken));
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
            return null;
        }
        catch (Exception e)
        {
            return null;
        }
    }

    public async Task<List<FullAlbum>> GetAllFavoriteAlbums()
    {
        try
        {
            var allAlbums = new List<FullAlbum>();

            await foreach (var savedAlbum in _spotify.Paginate(await _spotify.Library.GetAlbums()))
            {
                allAlbums.Add(savedAlbum.Album);
            }

            return allAlbums;
        }
        catch (APIException e)
        {
            return null;
        }
        catch (Exception e)
        {
            return null;
        }
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
            return null;
        }
        catch (Exception e)
        {
            return null;
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
            return null;
        }
        catch (Exception e)
        {
            return null;
        }
    }
}