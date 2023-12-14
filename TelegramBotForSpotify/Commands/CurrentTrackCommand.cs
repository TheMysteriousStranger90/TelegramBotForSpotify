﻿using TelegramBotForSpotify.Interfaces;
using TelegramBotForSpotify.Services;

namespace TelegramBotForSpotify.Commands;

public class CurrentTrackCommand : ICommand
{
    private readonly SpotifyService _spotifyService;
    private readonly TelegramService _telegramService;

    public CurrentTrackCommand(SpotifyService spotifyService, string botToken)
    {
        _spotifyService = spotifyService;
        _telegramService = TelegramService.Instance(botToken);
    }

    public async void Execute()
    {
        var track = await _spotifyService.GetCurrentTrack();
        if (track != null)
        {
            var message = $"Now playing: {track.Name} by {track.Artists[0].Name}";
            await _telegramService.SendMessage(chatId: "your_chat_id", text: message);
        }
    }
}