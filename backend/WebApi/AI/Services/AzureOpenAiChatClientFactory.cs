using System.ClientModel;
using Azure.AI.OpenAI;
using Azure.Identity;
using Microsoft.Extensions.Options;
using OpenAI.Chat;
using WebApi.AI.Interfaces;
using WebApi.Options;

namespace WebApi.AI.Services;

public class AzureOpenAiChatClientFactory : IChatClientFactory
{
    private readonly ChatClient _chatClient;

    public AzureOpenAiChatClientFactory(IOptions<AzureOpenAiOptions> options)
    {
        var config = options.Value;

        if (string.IsNullOrWhiteSpace(config.Endpoint))
            throw new InvalidOperationException("AzureOpenAI:Endpoint is not configured.");

        if (string.IsNullOrWhiteSpace(config.DeploymentName))
            throw new InvalidOperationException("AzureOpenAI:DeploymentName is not configured.");

        AzureOpenAIClient client;

        if (!string.IsNullOrWhiteSpace(config.ApiKey))
        {
            client = new AzureOpenAIClient(
                new Uri(config.Endpoint),
                new ApiKeyCredential(config.ApiKey));
        }
        else
        {
            client = new AzureOpenAIClient(
                new Uri(config.Endpoint),
                new DefaultAzureCredential());
        }

        _chatClient = client.GetChatClient(config.DeploymentName);
    }

    public ChatClient Create() => _chatClient;
}