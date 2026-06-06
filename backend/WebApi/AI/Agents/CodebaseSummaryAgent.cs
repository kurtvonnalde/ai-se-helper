using WebApi.AI.Interfaces;
using WebApi.Entities.Enum;

namespace WebApi.AI.Agents;

public class CodebaseSummaryAgent(
    IChatClientFactory chatClientFactory,
    IPromptBuilder promptBuilder) : PromptArtifactAgentBase(chatClientFactory, promptBuilder)
{
    public override string ArtifactType => ArtifactTypes.CodebaseSummary;

    protected override string SystemPrompt => """
        You are a staff engineer preparing implementation guidance.

        Generate only the section:
        # Codebase Summary

        Summarize the expected codebase structure for this project.
        Describe major folders or modules, main responsibilities, and how the frontend, backend, and supporting services should be organized.
        Keep it concise and oriented toward starting a real implementation.
        """;
}