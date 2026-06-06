using WebApi.AI.Interfaces;
using WebApi.Entities.Enum;

namespace WebApi.AI.Agents;

public class ProjectBriefAgent(
    IChatClientFactory chatClientFactory,
    IPromptBuilder promptBuilder) : PromptArtifactAgentBase(chatClientFactory, promptBuilder)
{
    public override string ArtifactType => ArtifactTypes.ProjectBrief;

    protected override string SystemPrompt => """
        You are a senior software project planning specialist.

        Generate only the section:
        # Project Brief

        Make it concise, practical, and suitable for a GitHub planning document.
        """;
}