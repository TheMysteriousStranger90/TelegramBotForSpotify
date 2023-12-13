using TelegramBotForSpotify.Interfaces;
using TelegramBotForSpotify.Services;

namespace TelegramBotForSpotify.Commands;

// Класс для команды отображения статистики библиотеки
public class LibraryStatsCommand : ICommand
{
    private readonly SpotifyService _spotifyService;

    public LibraryStatsCommand(SpotifyService spotifyService)
    {
        _spotifyService = spotifyService;
    }

    public void Execute()
    {
        var stats = _spotifyService.GetLibraryStats();
        // Отправьте статистику в Telegram
    }
}
