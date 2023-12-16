using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
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
        try
        {
            await _botClient.SendTextMessageAsync(chatId, text);
        }
        catch (ApiRequestException e)
        {
            Console.WriteLine($"An error occurred while sending message: {e.Message}");
        }
    }

    public async Task<List<Telegram.Bot.Types.Update>> GetUpdates(int offset = 0)
    {
        try
        {
            var updates = await _botClient.GetUpdatesAsync(offset);
            return updates.ToList();
        }
        catch (ApiRequestException e)
        {
            Console.WriteLine($"An error occurred while getting updates: {e.Message}");
            return new List<Telegram.Bot.Types.Update>();
        }
    }
}