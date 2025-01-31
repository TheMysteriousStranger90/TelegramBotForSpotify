﻿using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBotForSpotify.Auth;
using TelegramBotForSpotify.Commands;
using TelegramBotForSpotify.Interfaces;

namespace TelegramBotForSpotify.Services;

public class TelegramService : ITelegramService
{
    private readonly string _token;
    private readonly TelegramBotClient _botClient;

    public TelegramService(IOptions<TelegramSettings> settings)
    {
        _token = settings.Value.Token;
        _botClient = new TelegramBotClient(_token);
    }

    public async Task SendMessage(string chatId, string text)
    {
        try
        {
            await _botClient.SendTextMessageAsync(chatId, text);
        }
        catch (ApiRequestException e)
        {
            Console.WriteLine($"An error occurred while sending message: {e.Message}");
        }
    }

    public async Task<List<Telegram.Bot.Types.Update>> GetUpdates(int offset = 0)
    {
        try
        {
            var updates = await _botClient.GetUpdatesAsync(offset);
            return updates.ToList();
        }
        catch (ApiRequestException e)
        {
            Console.WriteLine($"An error occurred while getting updates: {e.Message}");
            return new List<Telegram.Bot.Types.Update>();
        }
    }
    
    public async Task SendPhotoAsync(string chatId, string photoUrl)
    {
        var photo = Telegram.Bot.Types.InputFile.FromUri(photoUrl);
        await _botClient.SendPhotoAsync(chatId, photo);
    }
    
    public async Task SendDocumentAsync(string chatId, string filePath, string caption = null)
    {
        try
        {
            using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            var fileName = Path.GetFileName(filePath);
            await _botClient.SendDocumentAsync(
                chatId: chatId,
                document: InputFile.FromStream(stream, fileName),
                caption: caption
            );
        }
        catch (ApiRequestException e)
        {
            Console.WriteLine($"An error occurred while sending document: {e.Message}");
        }
    }
    
    public async Task SendAudioAsync(string chatId, string audioPath, string caption = null)
    {
        try
        {
            using var stream = new FileStream(audioPath, FileMode.Open, FileAccess.Read);
            var fileName = Path.GetFileName(audioPath);
            await _botClient.SendAudioAsync(
                chatId: chatId,
                audio: InputFile.FromStream(stream, fileName),
                caption: caption
            );
        }
        catch (ApiRequestException e)
        {
            Console.WriteLine($"An error occurred while sending audio: {e.Message}");
        }
    }
}