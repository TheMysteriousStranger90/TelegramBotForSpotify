using SpotifyAPI.Web;

namespace TelegramBotForSpotify.Services;

// Класс для взаимодействия с API Spotify
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
}