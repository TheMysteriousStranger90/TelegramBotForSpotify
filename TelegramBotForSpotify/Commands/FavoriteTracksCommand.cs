using Telegram.Bot.Types;
using TelegramBotForSpotify.Interfaces;
using TelegramBotForSpotify.Services;

namespace TelegramBotForSpotify.Commands;

public class FavoriteTracksCommand : ICommand
{
    private readonly ISpotifyTrackService _spotifyTrackService;
    private readonly ITelegramService _telegramService;

    public FavoriteTracksCommand(ISpotifyTrackService spotifyTrackService, ITelegramService telegramService)
    {
        _spotifyTrackService = spotifyTrackService;
        _telegramService = telegramService;
    }

    public async Task Execute(Message message)
    {
        var allTracks = await _spotifyTrackService.GetAllFavoriteTracks();
        foreach (var trackInfo in allTracks)
        {
            var _message =
                $"Track: {trackInfo.Track.Name}\nArtist: {trackInfo.Track.Artists[0].Name}\nAlbum: {trackInfo.Track.Album.Name}";
            await _telegramService.SendMessage(message.Chat.Id.ToString(), text: _message);
            await Task.Delay(1000);
        }
    }
}