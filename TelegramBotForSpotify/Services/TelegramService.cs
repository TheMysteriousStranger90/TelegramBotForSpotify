using Telegram.Bot;
using TelegramBotForSpotify.Interfaces;

namespace TelegramBotForSpotify.Services;

public class TelegramService : ITelegramService
{
    private readonly TelegramBotClient _botClient;

    public TelegramService(string botToken)
    {
        _botClient = new TelegramBotClient(botToken);
    }

    public async Task SendMessage(string chatId, string text)
    {
        await _botClient.SendTextMessageAsync(chatId, text);
    }

    public async Task<List<Telegram.Bot.Types.Update>> GetUpdates(int offset = 0)
    {
        var updates = await _botClient.GetUpdatesAsync(offset);
        return updates.ToList();
    }
}