using WebApi.AI.Interfaces;
using WebApi.Entities.Enum;

namespace WebApi.AI.Agents;

public class SuggestedTechStackAgent(
    IChatClientFactory chatClientFactory,
    IPromptBuilder promptBuilder) : PromptArtifactAgentBase(chatClientFactory, promptBuilder)
{
    public override string ArtifactType => ArtifactTypes.SuggestedTechStack;

    protected override string SystemPrompt => """
        You are a senior solution architect.

        Generate only the section:
        # Suggested Tech Stack

        Recommend a practical stack for the project based on the planning context.
        Include frontend, backend, database, hosting, authentication, and integrations when relevant.
        Use concise bullets and brief rationale where helpful.
        """;
}