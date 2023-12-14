using TelegramBotForSpotify.Auth;
using TelegramBotForSpotify.Interfaces;
using TelegramBotForSpotify.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Add configuration
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

// Add Spotify and Telegram services
builder.Services.Configure<SpotifySettings>(builder.Configuration.GetSection("Spotify"));
builder.Services.Configure<TelegramSettings>(builder.Configuration.GetSection("Telegram"));
builder.Services.AddSingleton<ITelegramService, TelegramService>();
builder.Services.AddSingleton<ISpotifyAuthorizationService, SpotifyAuthorizationService>();
builder.Services.AddSingleton<ISpotifyAlbumService, SpotifyAlbumService>();
builder.Services.AddSingleton<ISpotifyPlaylistService, SpotifyPlaylistService>();
builder.Services.AddSingleton<ISpotifyTrackService, SpotifyTrackService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();