using SpotifyAPI.Web;
using Telegram.Bot.Types;
using TelegramBotForSpotify.Interfaces;

namespace TelegramBotForSpotify.Commands;

public class CommandHandler
{
    private readonly ITelegramService _telegramService;
    private readonly CurrentTrackCommand _currentTrackCommand;
    private readonly FavoriteAlbumsStatsCommand _favoriteAlbumsStatsCommand;
    private readonly FavoriteTracksCommand _favoriteTracksCommand;
    private readonly PlaylistInfoCommand _playlistInfoCommand;
    private readonly ISpotifyClientFactory _spotifyClientFactory;

    public CommandHandler(ISpotifyClientFactory spotifyClientFactory, ITelegramService telegramService,
        CurrentTrackCommand currentTrackCommand, FavoriteAlbumsStatsCommand favoriteAlbumsStatsCommand,
        FavoriteTracksCommand favoriteTracksCommand, PlaylistInfoCommand playlistInfoCommand)
    {
        _telegramService = telegramService;
        _currentTrackCommand = currentTrackCommand;
        _favoriteAlbumsStatsCommand = favoriteAlbumsStatsCommand;
        _favoriteTracksCommand = favoriteTracksCommand;
        _playlistInfoCommand = playlistInfoCommand;
        _spotifyClientFactory = spotifyClientFactory;
    }

    public async Task HandleCommand(Message message)
    {
        var spotifyClient = await _spotifyClientFactory.CreateSpotifyClientAsync();

        string command = message.Text.Split(' ')[0];

        switch (command)
        {
            case "/start":
                await _telegramService.SendMessage(message.Chat.Id.ToString(), "Bot started");
                break;
            case "/gettrack":
                await _currentTrackCommand.Execute();
                break;
            case "/gettracks":
                await _favoriteTracksCommand.Execute();
                break;
            case "/getalbums":
                await _favoriteAlbumsStatsCommand.Execute();
                break;
            case "/getplaylists":
                await _playlistInfoCommand.Execute();
                break;
            case "/help":
                var helpMessage = "Here are the available commands:\n" +
                                  "/start - Start the bot\n" +
                                  "/gettrack - Get the current track\n" +
                                  "/gettracks - Get favorite tracks\n" +
                                  "/getalbums - Get favorite albums\n" +
                                  "/getplaylists - Get playlists";
                await _telegramService.SendMessage(message.Chat.Id.ToString(), helpMessage);
                break;
            default:
                await _telegramService.SendMessage(message.Chat.Id.ToString(), "Unknown command");
                break;
        }
    }
}