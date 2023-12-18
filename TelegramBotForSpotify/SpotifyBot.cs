using TelegramBotForSpotify.Commands;
using TelegramBotForSpotify.Interfaces;

namespace TelegramBotForSpotify;

public class SpotifyBot
{
    private readonly ITelegramService _telegramService;
    private readonly CommandHandler _commandHandler;

    public SpotifyBot(ITelegramService telegramService, CommandHandler commandHandler)
    {
        _telegramService = telegramService;
        _commandHandler = commandHandler;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var offset = 0;

        while (!cancellationToken.IsCancellationRequested)
        {
            var updates = await _telegramService.GetUpdates(offset);

            foreach (var update in updates)
            {
                if (update.Message?.Text != null)
                {
                    var command = update.Message.Text.Split(' ').First();
                    await _commandHandler.HandleCommand(command, update);
                }

                offset = update.Id + 1;
            }

            await Task.Delay(1000);
        }
    }
}