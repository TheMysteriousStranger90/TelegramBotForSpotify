using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBotForSpotify.Interfaces;

namespace TelegramBotForSpotify.Commands;

public class HelpCommand : ICommand
{
    private readonly ITelegramService _telegramService;

    public HelpCommand(ITelegramService telegramService)
    {
        _telegramService = telegramService;
    }

    public async Task ExecuteAsync(Update update)
    {
        var message = update.Message;
        if (message != null && message.Type == MessageType.Text)
        {
            var _message = "Available commands:\n" +
                           "/gettrack - Get the current track playing on Spotify\n" +
                           "/gettracks - Get all your favorite tracks\n" +
                           "/getalbums - Get all your favorite albums\n" +
                           "/getplaylists - Get all your favorite playlists\n" +
                           "/help - Get all available commands";
            await _telegramService.SendMessage(message.Chat.Id.ToString(), text: _message);
        }
        else
        {
            await _telegramService.SendMessage(message.Chat.Id.ToString(),
                text: "Invalid command. Please send a text message.");
        }
    }
}