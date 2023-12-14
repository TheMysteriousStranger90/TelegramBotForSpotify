using TelegramBotForSpotify.Interfaces;
using TelegramBotForSpotify.Services;

namespace TelegramBotForSpotify.Commands;

public class FavoriteTracksCommand : ICommand
{
    private readonly ISpotifyTrackService _spotifyTrackService;
    private readonly TelegramService _telegramService;
    private readonly string _userId;

    public FavoriteTracksCommand(ISpotifyTrackService spotifyTrackService, TelegramService telegramService, string userId)
    {
        _spotifyTrackService = spotifyTrackService;
        _telegramService = telegramService;
        _userId = userId;
    }

    public async Task Execute()
    {
        var allTracks = await _spotifyTrackService.GetAllFavoriteTracks();
        foreach (var trackInfo in allTracks)
        {
            var message = $"Track: {trackInfo.Track.Name}\nArtist: {trackInfo.Track.Artists[0].Name}\nAlbum: {trackInfo.Track.Album.Name}";
            await _telegramService.SendMessage(chatId: "your_chat_id", text: message);
            await Task.Delay(1000);
        }
    }
}