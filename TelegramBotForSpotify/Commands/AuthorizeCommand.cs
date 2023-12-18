using Telegram.Bot.Types;
using TelegramBotForSpotify.Interfaces;

namespace TelegramBotForSpotify.Commands;

public class AuthorizeCommand : ICommand
{
    private readonly AuthorizeManager _authorizeManager;

    public AuthorizeCommand(AuthorizeManager authorizeManager)
    {
        _authorizeManager = authorizeManager;
    }

    public async Task ExecuteAsync(Update update)
    {
        await _authorizeManager.StartAuthorization(update.Message.Chat.Id);
    }
}