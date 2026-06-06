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

Provide a practical, modern, and production-ready technology stack based on the project context.

Structure the response using the following subsections:

## Frontend
List the recommended frontend framework, libraries, and key tools.

## Backend
Specify the backend framework, language, and architecture style.

## Database
Recommend the primary database and any supporting data storage solutions.

## Hosting & Infrastructure
Define where and how the application will be hosted (e.g., cloud services, CI/CD, environments).

## Authentication & Security
Describe authentication methods and key security practices.

## Integrations & Services
List external services, APIs, or AI components if relevant.

## Rationale
Briefly explain why this stack is a good fit for the project (focus on scalability, maintainability, and developer productivity).

Guidelines:
- Use concise bullet points under each section.
- Prefer industry-standard and widely supported technologies.
- Align recommendations with modern best practices.
- Keep it implementation-ready and realistic.
- Avoid unnecessary alternatives—focus on a single cohesive stack.
- Do NOT include any sections outside of those listed above.
- Do NOT add explanations before or after the output.
""";
}