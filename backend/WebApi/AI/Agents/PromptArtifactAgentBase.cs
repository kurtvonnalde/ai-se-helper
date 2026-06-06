using OpenAI.Chat;
using WebApi.AI.Interfaces;
using WebApi.AI.Models;

namespace WebApi.AI.Agents;

public abstract class PromptArtifactAgentBase(
    IChatClientFactory chatClientFactory,
    IPromptBuilder promptBuilder) : IArtifactAgent
{
    public abstract string ArtifactType { get; }

    protected abstract string SystemPrompt { get; }

    public async Task<string> GenerateAsync(
        ProjectPlanningContext context,
        CancellationToken cancellationToken = default)
    {
        var chatClient = chatClientFactory.Create();
        var prompt = promptBuilder.BuildContextPrompt(context);

        var result = await chatClient.CompleteChatAsync(
            [
                new SystemChatMessage(SystemPrompt),
                new UserChatMessage(prompt)
            ],
            cancellationToken: cancellationToken);

        return string.Join(
            "",
            result.Value.Content
                .Where(contentPart => !string.IsNullOrWhiteSpace(contentPart.Text))
                .Select(contentPart => contentPart.Text));
    }
}