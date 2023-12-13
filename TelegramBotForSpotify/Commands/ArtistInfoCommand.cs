using TelegramBotForSpotify.Interfaces;
using TelegramBotForSpotify.Services;

namespace TelegramBotForSpotify.Commands;

// Класс для команды отображения информации об исполнителе
public class ArtistInfoCommand : ICommand
{
    private readonly SpotifyService _spotifyService;
    private readonly string _artistName;

    public ArtistInfoCommand(SpotifyService spotifyService, string artistName)
    {
        _spotifyService = spotifyService;
        _artistName = artistName;
    }

    public void Execute()
    {
        var artistInfo = _spotifyService.GetArtistInfo(_artistName);
        // Отправьте информацию об исполнителе в Telegram
    }
}
