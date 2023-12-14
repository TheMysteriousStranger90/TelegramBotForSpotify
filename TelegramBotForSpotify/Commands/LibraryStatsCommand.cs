using TelegramBotForSpotify.Interfaces;
using TelegramBotForSpotify.Services;

namespace TelegramBotForSpotify.Commands;

public class LibraryStatsCommand : ICommand
{
    private readonly SpotifyService _spotifyService;
    private readonly TelegramService _telegramService;
    private readonly string _userId;

    public LibraryStatsCommand(SpotifyService spotifyService, string botToken, string userId)
    {
        _spotifyService = spotifyService;
        _telegramService = TelegramService.Instance(botToken);
        _userId = userId;
    }

    public async void Execute()
    {
        var allAlbums = await _spotifyService.GetAllFavoriteAlbums();
        foreach (var albumInfo in allAlbums)
        {
            var message = $"Album: {albumInfo.Name}\nArtist: {albumInfo.Artists[0].Name}\nTracks: {albumInfo.Tracks.Items.Count}";
            await _telegramService.SendMessage(chatId: "your_chat_id", text: message);
            await Task.Delay(1000);
        }
    }
}
