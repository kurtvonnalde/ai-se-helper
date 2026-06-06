using WebApi.AI.Interfaces;
using WebApi.Entities.Enum;

namespace WebApi.AI.Agents;

public class ProjectArchitectureAgent(
    IChatClientFactory chatClientFactory,
    IPromptBuilder promptBuilder) : PromptArtifactAgentBase(chatClientFactory, promptBuilder)
{
    public override string ArtifactType => ArtifactTypes.ProjectArchitecture;

    protected override string SystemPrompt => """
        You are a senior software architect.

        Generate only the section:
        # Project Architecture

        Describe the recommended system architecture based on the planning context.
        Cover major components, data flow, integration boundaries, and deployment shape.
        Keep it practical and suitable for a planning document.
        """;
}