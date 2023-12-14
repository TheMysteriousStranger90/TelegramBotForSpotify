using TelegramBotForSpotify.Interfaces;
using TelegramBotForSpotify.Services;

namespace TelegramBotForSpotify.Commands;

public class FavoriteTracksCommand : ICommand
{
    private readonly SpotifyService _spotifyService;
    private readonly TelegramService _telegramService;
    private readonly string _userId;

    public FavoriteTracksCommand(SpotifyService spotifyService, string botToken, string userId)
    {
        _spotifyService = spotifyService;
        _telegramService = TelegramService.Instance(botToken);
        _userId = userId;
    }

    public async void Execute()
    {
        var allTracks = await _spotifyService.GetAllFavoriteTracks();
        foreach (var trackInfo in allTracks)
        {
            var message = $"Track: {trackInfo.Track.Name}\nArtist: {trackInfo.Track.Artists[0].Name}\nAlbum: {trackInfo.Track.Album.Name}";
            await _telegramService.SendMessage(chatId: "your_chat_id", text: message);
            await Task.Delay(1000);
        }
    }
}