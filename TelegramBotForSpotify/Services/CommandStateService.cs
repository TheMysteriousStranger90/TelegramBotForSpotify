namespace TelegramBotForSpotify.Services;

public class CommandStateService
{
    private bool _isSending = true;

    public void StopSending()
    {
        _isSending = false;
    }

    public void StartSending()
    {
        _isSending = true;
    }

    public bool IsSending()
    {
        return _isSending;
    }
}