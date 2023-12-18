using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBotForSpotify.Commands;
using TelegramBotForSpotify.Interfaces;
using TelegramBotForSpotify.Services;
using File = System.IO.File;

namespace TelegramBotForSpotify;

public class Bot
{
    private readonly AuthorizeManager _authorizeManager;
    private readonly CommandHandler _commandHandler;
    private readonly ITelegramService _telegramService;
    private const string LastUpdateIdFilePath = "wwwroot/data/lastUpdateId.json";
    private int _lastUpdateId;

    public Bot(AuthorizeManager authorizeManager, CommandHandler commandHandler, ITelegramService telegramService)
    {
        _authorizeManager = authorizeManager;
        _commandHandler = commandHandler;
        _telegramService = telegramService;
        _lastUpdateId = LoadLastUpdateId();
    }

    public async Task Start()
    {
        var updates = await _telegramService.GetUpdates(_lastUpdateId + 1);

        Console.WriteLine($"Received {updates.Count} updates from Telegram.");

        int maxUpdateId = _lastUpdateId;

        foreach (var update in updates)
        {
            maxUpdateId = Math.Max(maxUpdateId, update.Id);
            if (update.Message != null)
            {
                var chatId = update.Message.Chat.Id;
                await _authorizeManager.StartAuthorization(chatId);
                Console.WriteLine($"Received message: {update.Message.Text} from {chatId} time {update.Message.Date}.");
            }
        }

        _lastUpdateId = maxUpdateId;

        SaveLastUpdateId(_lastUpdateId);
    }

    private int LoadLastUpdateId()
    {
        if (File.Exists(LastUpdateIdFilePath))
        {
            var lastUpdateIdStr = File.ReadAllText(LastUpdateIdFilePath);
            if (int.TryParse(lastUpdateIdStr, out var lastUpdateId))
            {
                return lastUpdateId;
            }
        }

        return 0;
    }

    private void SaveLastUpdateId(int lastUpdateId)
    {
        var json = System.Text.Json.JsonSerializer.Serialize(lastUpdateId);
        File.WriteAllText(LastUpdateIdFilePath, json);
    }
}