using Telegram.Bot.Types;
using TelegramBotForSpotify.Interfaces;

namespace TelegramBotForSpotify.Commands;

public class CommandHandler
{
    private readonly ITelegramService _telegramService;
    private readonly CurrentTrackCommand _currentTrackCommand;
    private readonly FavoriteAlbumsStatsCommand _favoriteAlbumsStatsCommand;
    private readonly FavoriteTracksCommand _favoriteTracksCommand;
    private readonly PlaylistInfoCommand _playlistInfoCommand;

    public CommandHandler(ITelegramService telegramService, CurrentTrackCommand currentTrackCommand, FavoriteAlbumsStatsCommand favoriteAlbumsStatsCommand, FavoriteTracksCommand favoriteTracksCommand, PlaylistInfoCommand playlistInfoCommand)
    {
        _telegramService = telegramService;
        _currentTrackCommand = currentTrackCommand;
        _favoriteAlbumsStatsCommand = favoriteAlbumsStatsCommand;
        _favoriteTracksCommand = favoriteTracksCommand;
        _playlistInfoCommand = playlistInfoCommand;
    }

    public async Task HandleCommand(Message message)
    {
        string command = message.Text.Split(' ')[0];

        switch (command)
        {
            case "/start":
                await _telegramService.SendMessage(message.Chat.Id.ToString(), "Bot started");
                break;
            case "/gettrack":
                await _currentTrackCommand.Execute();
                break;
            case "/gettracks":
                await _favoriteTracksCommand.Execute();
                break;
            case "/getalbums":
                await _favoriteAlbumsStatsCommand.Execute();
                break;
            case "/getplaylists":
                await _playlistInfoCommand.Execute();
                break;

            default:
                await _telegramService.SendMessage(message.Chat.Id.ToString(), "Unknown command");
                break;
        }
    }
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    /*
    private readonly ITelegramService _telegramService;
    private readonly ISpotifyAlbumService _spotifyAlbumService;
    private readonly ISpotifyPlaylistService _spotifyPlaylistService;
    private readonly ISpotifyTrackService _spotifyTrackService;

    public CommandHandler(ITelegramService telegramService, ISpotifyAlbumService spotifyAlbumService, ISpotifyPlaylistService spotifyPlaylistService, ISpotifyTrackService spotifyTrackService)
    {
        _telegramService = telegramService;
        _spotifyAlbumService = spotifyAlbumService;
        _spotifyPlaylistService = spotifyPlaylistService;
        _spotifyTrackService = spotifyTrackService;
    }

    public async Task HandleCommand(Message message)
    {
        string command = message.Text.Split(' ')[0];

        switch (command)
        {
            case "/start":
                await _telegramService.SendMessage(message.Chat.Id.ToString(), "Bot started");
                break;
            case "/gettrack":
                var track = await _spotifyTrackService.GetCurrentTrack();
                if (track != null)
                {
                    await _telegramService.SendMessage(message.Chat.Id.ToString(), $"Now playing: {track.Name} by {track.Artists[0].Name}");
                }
                else
                {
                    await _telegramService.SendMessage(message.Chat.Id.ToString(), "No track is currently playing.");
                }
                break;
            case "/gettracks":
                var tracks = await _spotifyTrackService.GetAllFavoriteTracks();
                foreach (var savedTrack in tracks)
                {
                    var _track = savedTrack.Track;
                    await _telegramService.SendMessage(message.Chat.Id.ToString(), $"Track: {_track.Name} by {_track.Artists[0].Name}");
                }
                break;
            case "/getalbums":
                var albums = await _spotifyAlbumService.GetAllFavoriteAlbums();
                foreach (var album in albums)
                {
                    await _telegramService.SendMessage(message.Chat.Id.ToString(), $"Album: {album.Album.Name} by {album.Album.Artists}");
                }
                break;
            case "/getplaylists":
                string userId = message.From.Id.ToString();
                var playlists = await _spotifyPlaylistService.GetAllFavoritePlaylists(userId);
                foreach (var playlist in playlists)
                {
                    await _telegramService.SendMessage(message.Chat.Id.ToString(), $"Playlist: {playlist.Name} with {playlist.Tracks.Total} tracks");
                }
                break;

            default:
                await _telegramService.SendMessage(message.Chat.Id.ToString(), "Unknown command");
                break;
        }
    }
    */
    
    
}