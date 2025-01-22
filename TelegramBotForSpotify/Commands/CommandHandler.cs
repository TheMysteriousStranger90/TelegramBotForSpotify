using SpotifyAPI.Web;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBotForSpotify.Interfaces;
using TelegramBotForSpotify.Services;

namespace TelegramBotForSpotify.Commands;

public class CommandHandler
{
    private readonly Dictionary<string, ICommand> _commands;
    private readonly AuthorizeManager _authorizeManager;
    private readonly ITelegramService _telegramService;

    public CommandHandler(AuthorizeManager authorizeManager, ISpotifyTrackService spotifyTrackService, ISpotifyTracksService spotifyTracksService,
        ISpotifyAlbumService spotifyAlbumService, ISpotifyPlaylistService spotifyPlaylistService,
        ITelegramService telegramService)
    {
        _authorizeManager = authorizeManager;
        _telegramService = telegramService;
        _commands = new Dictionary<string, ICommand>
        {
            { "/authorize", new AuthorizeCommand(authorizeManager) },
            { "/gettrack", new CurrentTrackCommand(spotifyTrackService, telegramService) },
            { "/gettracks", new FavoriteTracksCommand(spotifyTracksService, telegramService) },
            { "/getalbums", new FavoriteAlbumsStatsCommand(spotifyAlbumService, telegramService) },
            { "/getplaylists", new PlaylistInfoCommand(spotifyPlaylistService, telegramService) },
            { "/exportalbums", new ExportFavoriteAlbumsCommand(spotifyAlbumService, telegramService) },
            { "/exporttracks", new ExportFavoriteTracksCommand(spotifyTracksService, telegramService) },
            { "/exportplaylists", new ExportPlaylistsCommand(spotifyPlaylistService, telegramService) },
            { "/start", new HelpCommand(telegramService) },
            { "/help", new HelpCommand(telegramService) }
        };
    }

    public async Task HandleCommand(string command, Update update)
    {
        if (_commands.TryGetValue(command, out var commandHandler))
        {
            if (command != "/authorize" && !(await _authorizeManager.IsAuthorized(update.Message.Chat.Id)))
            {
                await _telegramService.SendMessage(update.Message.Chat.Id.ToString(),
                    "Please authorize first by using the /authorize command.");
            }
            else
            {
                await commandHandler.ExecuteAsync(update);
            }
        }
        else
        {
            await _telegramService.SendMessage(update.Message.Chat.Id.ToString(),
                $"Unknown command: {command}. Please use /help to get a list of available commands.");
        }
    }
}