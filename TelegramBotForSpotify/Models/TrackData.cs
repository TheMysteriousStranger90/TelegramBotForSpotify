namespace TelegramBotForSpotify.Models;

public class TrackData
{
    public string Name { get; set; }
    public string Artist { get; set; }
    public string Album { get; set; }
    public string Duration { get; set; }
    public bool IsExplicit { get; set; }
    public string SpotifyUrl { get; set; }
}