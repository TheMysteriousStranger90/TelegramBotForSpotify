namespace TelegramBotForSpotify.Models;

public class PlaylistData
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int? TracksCount { get; set; }
    public bool IsPublic { get; set; }
    public string Owner { get; set; }
    public string SpotifyUrl { get; set; }
    public string ImageUrl { get; set; }
}