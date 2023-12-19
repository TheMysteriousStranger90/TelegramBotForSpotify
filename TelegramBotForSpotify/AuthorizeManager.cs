using TelegramBotForSpotify.Interfaces;

namespace TelegramBotForSpotify;

public class AuthorizeManager
{
    private readonly ISpotifyAuthorizationService _spotifyAuthService;
    private readonly ITelegramService _telegramService;
    private readonly Dictionary<long, bool> _authorizationStatus;

    public AuthorizeManager(ISpotifyAuthorizationService spotifyAuthService, ITelegramService telegramService)
    {
        _spotifyAuthService = spotifyAuthService;
        _telegramService = telegramService;
        _authorizationStatus = new Dictionary<long, bool>();
    }

    public async Task StartAuthorization(long chatId)
    {
        if (IsAuthorizationStarted(chatId))
        {
            return;
        }

        var authUrl = _spotifyAuthService.GetAuthorizationUrl(chatId.ToString());
        Console.WriteLine($"Starting authorization for chatId {chatId}. Authorization URL: {authUrl}");

        await _telegramService.SendMessage(chatId.ToString(),
            $"Please authorize the app by visiting the following URL: {authUrl}");

        _authorizationStatus[chatId] = false;
    }

    public async Task HandleCallback(string code, string state)
    {
        Console.WriteLine($"Received callback with code {code} and state {state}");

        var token = await _spotifyAuthService.Authorize(code);
        _spotifyAuthService.InitializeSpotifyClient(token);
        await _telegramService.SendMessage(state, $"Authorization successful. Your token is: {token}");
        
        await _telegramService.SendMessage(state, $"Please use /help to get a list of available commands.");

        if (long.TryParse(state, out var chatId))
        {
            Console.WriteLine($"State parsed to chatId: {chatId}");

            if (_authorizationStatus.ContainsKey(chatId))
            {
                Console.WriteLine($"ChatId exists in the dictionary: {chatId}");
                _authorizationStatus[chatId] = true;
            }
            else
            {
                Console.WriteLine($"ChatId does not exist in the dictionary: {chatId}");
            }
        }
        else
        {
            Console.WriteLine($"Failed to parse state to chatId. State: {state}");
        }
    }

    public async Task<bool> IsAuthorized(long chatId)
    {
        var isAuthorized = _authorizationStatus.TryGetValue(chatId, out var authStatus) && authStatus;
        Console.WriteLine($"IsAuthorized called for chatId {chatId}. Authorization status: {isAuthorized}");
        return isAuthorized;
    }

    public bool IsAuthorizationStarted(long chatId)
    {
        return _authorizationStatus.ContainsKey(chatId);
    }
}