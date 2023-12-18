using SpotifyAPI.Web;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBotForSpotify.Interfaces;

namespace TelegramBotForSpotify.Commands;

public class CommandHandler
{
    private readonly Dictionary<string, ICommand> _commands;
    private readonly AuthorizeManager _authorizeManager;
    private readonly ITelegramService _telegramService;

    public CommandHandler(AuthorizeManager authorizeManager, ISpotifyTrackService spotifyAuthService, ITelegramService telegramService)
    {
        _authorizeManager = authorizeManager;
        _telegramService = telegramService;
        _commands = new Dictionary<string, ICommand>
        {
            { "_authorize", new AuthorizeCommand(authorizeManager) },
            { "_current", new CurrentTrackCommand(spotifyAuthService, telegramService) }
        };
    }

    public async Task HandleCommand(string command, Update update)
    {
        if (_commands.TryGetValue(command, out var commandHandler))
        {
            if (command != "_authorize" && !(await _authorizeManager.IsAuthorized(update.Message.Chat.Id)))
            {
                await _telegramService.SendMessage(update.Message.Chat.Id.ToString(), "Please authorize first by using the /authorize command.");
            }
            else
            {
                await commandHandler.ExecuteAsync(update);
            }
        }
        else
        {
            // Handle unknown command
        }
    }
    
    
    
    
    
    
    
    
    
    
    
    /*
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

    
    
    public async Task HandleCommand(Update update)
    {
        string command = update.Message.Text.Split(' ')[0];


        switch (command)
        {
            case "/start":
                await _telegramService.SendMessage(update.Message.Chat.Id.ToString(), "Bot started");
                break;
            case "/gettrack":
                await _currentTrackCommand.ExecuteAsync(update);
                break;
            case "/gettracks":
                await _favoriteTracksCommand.ExecuteAsync(update);
                break;
            case "/getalbums":
                await _favoriteAlbumsStatsCommand.ExecuteAsync(update);
                break;
            case "/getplaylists":
                await _playlistInfoCommand.ExecuteAsync(update);
                break;
            case "/help":
                var helpMessage = "Here are the available commands:\n" +
                                  "/start - Start the bot\n" +
                                  "/gettrack - Get the current track\n" +
                                  "/gettracks - Get favorite tracks\n" +
                                  "/getalbums - Get favorite albums\n" +
                                  "/getplaylists - Get playlists";
                await _telegramService.SendMessage(update.Message.Chat.Id.ToString(), helpMessage);
                break;
            default:
                await _telegramService.SendMessage(update.Message.Chat.Id.ToString(), "Unknown command");
                break;
        }
    }

    public async Task HandleUpdate(Update update)
    {
        if (update.Type == UpdateType.Message)
        {
            await HandleCommand(update);
        }
    }
    */
}