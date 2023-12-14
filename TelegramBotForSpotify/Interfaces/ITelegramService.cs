namespace TelegramBotForSpotify.Interfaces;

public interface ITelegramService
{
    Task SendMessage(string chatId, string text);
    Task<List<Telegram.Bot.Types.Update>> GetUpdates(int offset = 0);
}