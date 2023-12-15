﻿using TelegramBotForSpotify.Interfaces;
using TelegramBotForSpotify.Services;

namespace TelegramBotForSpotify.Commands;

public class FavoriteAlbumsStatsCommand : ICommand
{
    private readonly ISpotifyAlbumService _spotifyAlbumService;
    private readonly ITelegramService _telegramService;

    public FavoriteAlbumsStatsCommand(ISpotifyAlbumService spotifyAlbumService, ITelegramService telegramService)
    {
        _spotifyAlbumService = spotifyAlbumService;
        _telegramService = telegramService;
    }

    public async Task Execute()
    {
        var allAlbums = await _spotifyAlbumService.GetAllFavoriteAlbums();
        foreach (var albumInfo in allAlbums)
        {
            var message =
                $"Album: {albumInfo.Album.Name}\nArtist: {albumInfo.Album.Artists[0].Name}\nTracks: {albumInfo.Album.Tracks.Items.Count}";
            await _telegramService.SendMessage(chatId: "your_chat_id", text: message);
            await Task.Delay(1000);
        }
    }
}