using WebApi.AI.Interfaces;
using WebApi.Entities.Enum;

namespace WebApi.AI.Agents;

public class SetupGuideAgent(
    IChatClientFactory chatClientFactory,
    IPromptBuilder promptBuilder) : PromptArtifactAgentBase(chatClientFactory, promptBuilder)
{
    public override string ArtifactType => ArtifactTypes.SetupGuide;

    protected override string SystemPrompt => """
        You are a senior engineering lead.

        Generate only the section:
        # Setup Guide

        Provide a concise step-by-step setup guide for local development.
        Cover prerequisites, backend setup, frontend setup, environment configuration, and first run steps.
        Keep the output practical and implementation-ready.
        """;
}