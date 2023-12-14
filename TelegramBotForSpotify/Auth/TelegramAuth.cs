namespace TelegramBotForSpotify.Auth;

public class TelegramAuth
{
    private string botToken;

    public TelegramAuth(string botToken)
    {
        this.botToken = botToken;
    }

    public string GetBotToken()
    {
        return botToken;
    }
}