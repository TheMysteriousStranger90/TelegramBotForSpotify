using TelegramBotForSpotify.Interfaces;

namespace TelegramBotForSpotify;

public class AuthorizeManager
{
    private readonly ISpotifyAuthorizationService _spotifyAuthService;
    private readonly ITelegramService _telegramService;

    public AuthorizeManager(ISpotifyAuthorizationService spotifyAuthService, ITelegramService telegramService)
    {
        _spotifyAuthService = spotifyAuthService;
        _telegramService = telegramService;
    }

    public async Task StartAuthorization(long chatId)
    {
        var authUrl = _spotifyAuthService.GetAuthorizationUrl(chatId.ToString());
        await _telegramService.SendMessage(chatId.ToString(),
            $"Please authorize the app by visiting the following URL: {authUrl}");
    }

    public async Task HandleCallback(string code, string state)
    {
        var token = await _spotifyAuthService.Authorize(code);
        _spotifyAuthService.InitializeSpotifyClient(token);
        await _telegramService.SendMessage(state, $"Authorization successful. Your token is: {token}");
    }
}