using TelegramBotForSpotify.Interfaces;
using TelegramBotForSpotify.Services;

namespace TelegramBotForSpotify.Commands;

public class CurrentTrackCommand : ICommand
{
    private readonly ISpotifyTrackService _spotifyTrackService;
    private readonly TelegramService _telegramService;

    public CurrentTrackCommand(ISpotifyTrackService spotifyTrackService, TelegramService telegramService)
    {
        _spotifyTrackService = spotifyTrackService;
        _telegramService = telegramService;
    }

    public async Task Execute()
    {
        var track = await _spotifyTrackService.GetCurrentTrack();
        if (track != null)
        {
            var message = $"Now playing: {track.Name} by {track.Artists[0].Name}";
            await _telegramService.SendMessage(chatId: "your_chat_id", text: message);
        }
    }
}