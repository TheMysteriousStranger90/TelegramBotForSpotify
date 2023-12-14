using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;
using TelegramBotForSpotify.Commands;

namespace TelegramBotForSpotify.Controllers;

public class MainController : Controller
{
    private readonly CommandHandler _commandHandler;

    public MainController(CommandHandler commandHandler)
    {
        _commandHandler = commandHandler;
    }

    public async Task<IActionResult> HandleCommand(Message message)
    {
        await _commandHandler.HandleCommand(message);
        return Ok();
    }
}