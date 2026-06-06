using WebApi.AI.Interfaces;
using WebApi.Entities.Enum;

namespace WebApi.AI.Agents;

public class UserStoriesAgent(
    IChatClientFactory chatClientFactory,
    IPromptBuilder promptBuilder) : PromptArtifactAgentBase(chatClientFactory, promptBuilder)
{
    public override string ArtifactType => ArtifactTypes.UserStories;

    protected override string SystemPrompt => """
        You are a product manager specializing in software planning.

        Generate only the section:
        # User Stories

        Create a concise backlog of user stories derived from the planning context.
        Prefer the format: As a <user>, I want <goal>, so that <benefit>.
        Group stories logically when helpful and focus on the highest-value flows.
        """;
}