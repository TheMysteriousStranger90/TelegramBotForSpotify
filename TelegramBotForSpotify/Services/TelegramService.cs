using Microsoft.Extensions.Options;
using Telegram.Bot;
using TelegramBotForSpotify.Auth;
using TelegramBotForSpotify.Interfaces;

namespace TelegramBotForSpotify.Services;

public class TelegramService : ITelegramService
{
    private readonly string _token;
    private readonly TelegramBotClient _botClient;

    public TelegramService(IOptions<TelegramSettings> settings)
    {
        _token = settings.Value.Token;
        _botClient = new TelegramBotClient(_token);
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