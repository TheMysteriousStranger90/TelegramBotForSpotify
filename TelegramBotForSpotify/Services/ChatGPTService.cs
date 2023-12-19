using System.Text;
using Microsoft.Extensions.Options;
using OpenAI.API;
using OpenAI.API.Completions;
using OpenAI.API.Models;
using TelegramBotForSpotify.Auth;

namespace TelegramBotForSpotify.Services;

public class ChatGPTService
{
    private readonly string _token;
    private readonly OpenAIAPI _openAiApi;

    public ChatGPTService(IOptions<ChatGPTSettings> settings)
    {
        _token = settings.Value.openAIKey;
        _openAiApi = new OpenAIAPI(_token);
    }

    public async Task<string> GetAIResponse(string message)
    {
        var builder = new StringBuilder();
        await foreach (var token in _openAiApi.Completions.StreamCompletionEnumerableAsync(
                           new CompletionRequest(message, Model.DavinciText, 1000, 0.5,
                               presencePenalty: 0.1,
                               frequencyPenalty: 0.1)))
        {
            builder.Append(token);
        }

        return builder.ToString();
    }
}
