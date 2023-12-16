using Telegram.Bot.Types;

namespace TelegramBotForSpotify.Interfaces;

public interface ICommand
{
    Task Execute(Message message);
}