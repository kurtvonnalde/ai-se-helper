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

Provide a clear and practical overview of how the codebase should be structured for a real-world implementation.

Structure the response using the following subsections:

## Repository Structure
Outline the top-level folders (e.g., frontend, backend, infrastructure, docs) and their purpose.

## Frontend Structure
Describe how the frontend code should be organized (e.g., pages, components, services, hooks, state management).

## Backend Structure
Explain the backend architecture (e.g., controllers, services, domain, data access, middleware).

## Shared & Cross-Cutting Concerns
Highlight shared code such as models, DTOs, utilities, configuration, and constants.

## Integrations & External Services
Show where and how external services (e.g., APIs, AI services, messaging, storage) are integrated within the codebase.

## Development & Deployment Files
List important supporting files (e.g., environment configs, CI/CD pipelines, Docker files, README).

Guidelines:
- Use concise bullet points for each section.
- Keep it implementation-focused and realistic for a production-ready project.
- Follow clean architecture or modular design principles where appropriate.
- Ensure clear separation of concerns between frontend, backend, and infrastructure.
- Avoid unnecessary complexity—favor clarity and scalability.
- Do NOT include any sections outside of those listed above.
- Do NOT add explanations before or after the output.
""";

}