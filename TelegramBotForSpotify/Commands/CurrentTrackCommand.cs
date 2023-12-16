using Telegram.Bot.Types;
using TelegramBotForSpotify.Interfaces;
using TelegramBotForSpotify.Services;

namespace TelegramBotForSpotify.Commands;

public class CurrentTrackCommand : ICommand
{
    private readonly ISpotifyTrackService _spotifyTrackService;
    private readonly ITelegramService _telegramService;

    public CurrentTrackCommand(ISpotifyTrackService spotifyTrackService, ITelegramService telegramService)
    {
        _spotifyTrackService = spotifyTrackService;
        _telegramService = telegramService;
    }

    public async Task Execute(Message message)
    {
        var track = await _spotifyTrackService.GetCurrentTrack();
        if (track != null)
        {
            var _message = $"Now playing: {track.Name} by {track.Artists[0].Name}";
            await _telegramService.SendMessage(message.Chat.Id.ToString(), text: _message);
        }
    }
}