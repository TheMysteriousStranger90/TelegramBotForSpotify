using TelegramBotForSpotify.Commands;
using TelegramBotForSpotify.Interfaces;

namespace TelegramBotForSpotify;

public class Bot
{
    private readonly AuthorizeManager _authorizeManager;
    private readonly ITelegramService _telegramService;
    private readonly CommandHandler _commandHandler;

    public Bot(AuthorizeManager authorizeManager, ITelegramService telegramService, CommandHandler commandHandler)
    {
        _authorizeManager = authorizeManager;
        _telegramService = telegramService;
        _commandHandler = commandHandler;
    }

    public async Task Start()
    {
        var updates = await _telegramService.GetUpdates();

        foreach (var update in updates)
        {
            if (update.Message != null)
            {
                if (update.Message.Text == "/start")
                {
                    var chatId = update.Message.Chat.Id;
                    _authorizeManager.StartAuthorization(chatId);
                }
                else
                {
                    _commandHandler.HandleCommand(update.Message);
                }
            }
        }
    }
}