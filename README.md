# TelegramBotForSpotify

![Image 1](Screenshots/Screen1.png)

This project is a Telegram bot that interacts with the Spotify API to provide various functionalities. The bot can authorize a user, fetch the current track, favorite tracks, favorite albums, and playlist information.

## Commands
The bot supports the following commands:
```
/authorize: Authorizes the user.
/gettrack: Fetches the current track.
/gettracks: Fetches the user's favorite tracks.
/getalbums: Fetches the user's favorite albums.
/getplaylists: Fetches the user's playlists.
/help: Provides a list of available commands.
```
## Setup
To run this project, you need to create an appsettings.json file in the root directory with the following structure:
```
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "Spotify": {
    "ClientId": "<Your Spotify Client ID>",
    "ClientSecret": "<Your Spotify Client Secret>",
    "RedirectUri": "http://localhost:8000/callback"
  },
  "Telegram": {
    "Token": "<Your Telegram Bot Token>"
  }
}
```
Replace "<Example>" <Your Spotify Client Secret>, and <Your Telegram Bot Token> with your actual Spotify Client ID, Spotify Client Secret, and Telegram Bot Token respectively.

## Running the Project
After setting up the appsettings.json file, you can run the project using your preferred .NET Core-compatible IDE or from the command line with the dotnet run command.

## Running the Application with Docker

1. Make sure you have Docker installed on your machine. If not, you can download it from [here](https://www.docker.com/products/docker-desktop).

2. Navigate to the root directory of the project in your terminal.

3. Build the Docker image from the Dockerfile:

    ```bash
    docker build -t telegrambotforspotify .
    ```

4. Run the Docker container, mapping the application's port 80 to port 5001 on the host machine:

    ```bash
    docker run -p 8000:80 -d telegrambotforspotify
    ```

5. Open a Telegram, find StrangerSpotifyBot and start it.

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

## Author

Bohdan Harabadzhyu

## License

[MIT](https://choosealicense.com/licenses/mit/)