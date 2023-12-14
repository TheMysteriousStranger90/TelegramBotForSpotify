using TelegramBotForSpotify.Auth;
using TelegramBotForSpotify.Commands;
using TelegramBotForSpotify.Services;

namespace TelegramBotForSpotify.Helpers;

public class Bot
{
    private readonly AuthorizeManager _authorizeManager;
    private readonly TelegramService _telegramService;

    public Bot(AuthorizeManager authorizeManager, TelegramService telegramService)
    {
        _authorizeManager = authorizeManager;
        _telegramService = telegramService;
    }

    public async Task Start()
    {
        var updates = await _telegramService.GetUpdates();

        foreach (var update in updates)
        {
            if (update.Message != null)
            {
                var chatId = update.Message.Chat.Id;
                var state = "state";
                _authorizeManager.Authorize(chatId, state);
            }
        }
    }
}