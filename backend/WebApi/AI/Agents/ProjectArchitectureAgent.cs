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

Provide a clear, practical, and production-ready system architecture based on the project context.

Structure the response using the following subsections:

## Architecture Overview
Provide a high-level description of the system and its architectural style (e.g., layered, microservices, modular monolith).

## High-Level Components
List the major system components (e.g., frontend, backend, APIs, data stores, external services) and their responsibilities.

## Data Flow
Explain how data moves through the system from user interaction to storage and back.

## Integration Boundaries
Describe how the system interacts with external services (e.g., APIs, AI services, authentication providers).

## Deployment Architecture
Outline how the system is deployed (e.g., cloud services, environments, scaling strategy).

## Architecture Diagram
Provide a visual diagram as an ASCII text diagram (NOT Mermaid).

- Use a boxed text layout similar to terminal-style architecture diagrams.
- Clearly label all components and connections.
- Show data flow direction between frontend, backend, database, and external services.
- Always wrap the diagram in a fenced code block using plain text formatting.
- Do not use Mermaid keywords (e.g., flowchart, graph, sequenceDiagram) anywhere.

Example format (adapt as needed):

```text
+--------+      +----------+      +----------+
|  User  | ---> | Frontend | ---> | Backend  |
+--------+      +----------+      +----------+
                                   |
                                   v
                              +----------+
                              | Database |
                              +----------+
```
""";
}