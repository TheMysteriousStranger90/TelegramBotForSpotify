﻿using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBotForSpotify.Interfaces;
using TelegramBotForSpotify.Services;

namespace TelegramBotForSpotify.Commands;

public class FavoriteTracksCommand : ICommand
{
    private readonly ISpotifyTracksService _spotifyTracksService;
    private readonly ITelegramService _telegramService;

    public FavoriteTracksCommand(ISpotifyTracksService spotifyTracksService, ITelegramService telegramService)
    {
        _spotifyTracksService = spotifyTracksService;
        _telegramService = telegramService;
    }

    public async Task ExecuteAsync(Update update)
    {
        var message = update.Message;
        if (message != null && message.Type == MessageType.Text)
        {
            var allTracks = await _spotifyTracksService.GetAllFavoriteTracks();
            foreach (var trackInfo in allTracks)
            {
                var _message =
                    $"Track: {trackInfo.Track.Name}\nArtist: {trackInfo.Track.Artists[0].Name}\nAlbum: {trackInfo.Track.Album.Name}";
                await _telegramService.SendMessage(message.Chat.Id.ToString(), text: _message);
                await Task.Delay(1000);
            }
        }
        else
        {
            var _message = "No favorite tracks.";
            await _telegramService.SendMessage(message.Chat.Id.ToString(), text: _message);
        }
    }
}