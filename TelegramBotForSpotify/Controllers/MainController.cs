using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;
using TelegramBotForSpotify.Commands;

namespace TelegramBotForSpotify.Controllers;

public class MainController : Controller
{
    private readonly CommandHandler _commandHandler;

    private readonly AuthorizeManager _authorizeManager;

    public MainController(AuthorizeManager authorizeManager)
    {
        _authorizeManager = authorizeManager;
    }

    [HttpGet("callback")]
    public async Task<IActionResult> HandleSpotifyCallback(string code, string state)
    {
        await _authorizeManager.HandleCallback(code, state);
        return Ok();
    }
}