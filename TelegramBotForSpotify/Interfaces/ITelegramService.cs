﻿using TelegramBotForSpotify.Commands;

namespace TelegramBotForSpotify.Interfaces;

public interface ITelegramService
{
    Task SendMessage(string chatId, string text);
    Task<List<Telegram.Bot.Types.Update>> GetUpdates(int offset = 0);
    Task SendPhotoAsync(string chatId, string photoUrl);
}