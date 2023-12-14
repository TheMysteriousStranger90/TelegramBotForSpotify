using Telegram.Bot;

namespace TelegramBotForSpotify.Services;

public class TelegramService
{
    private static TelegramService instance = null;
    private readonly ITelegramBotClient _botClient;

    private TelegramService(string botToken)
    {
        _botClient = new TelegramBotClient(botToken);
    }

    public static TelegramService Instance(string botToken)
    {
        if (instance == null)
        {
            instance = new TelegramService(botToken);
        }

        return instance;
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