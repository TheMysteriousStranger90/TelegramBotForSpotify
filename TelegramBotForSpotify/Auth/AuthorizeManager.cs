using TelegramBotForSpotify.Interfaces;

namespace TelegramBotForSpotify.Auth;

public class AuthorizeManager
{
    private readonly ISpotifyAuthorizationService _spotifyAuthService;
    private readonly ITelegramService _telegramService;

    public AuthorizeManager(ISpotifyAuthorizationService spotifyAuthService, ITelegramService telegramService)
    {
        _spotifyAuthService = spotifyAuthService;
        _telegramService = telegramService;
    }

    public async void Authorize(long chatId, string state)
    {
        var authUrl = _spotifyAuthService.GetAuthorizationUrl(state);
        await _telegramService.SendMessage(chatId.ToString(), $"Please authorize the app by visiting the following URL: {authUrl}");
    }
}