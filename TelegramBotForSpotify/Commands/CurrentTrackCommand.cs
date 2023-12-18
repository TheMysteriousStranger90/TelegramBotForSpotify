using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
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

    public async Task ExecuteAsync(Update update)
    {
        var message = update.Message;
        if (message != null)
        {
            var track = await _spotifyTrackService.GetCurrentTrack();
            if (track != null)
            {
                var _message = $"Now playing: {track.Name} by {track.Artists[0].Name}";
                await _telegramService.SendMessage(message.Chat.Id.ToString(), text: _message);
            }
            else
            {
                var _message = "No track is currently playing.";
                await _telegramService.SendMessage(message.Chat.Id.ToString(), text: _message);
            }
        }
    }
}