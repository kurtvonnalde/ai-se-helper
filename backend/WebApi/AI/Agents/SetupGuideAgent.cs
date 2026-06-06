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

Provide a concise, step-by-step guide for setting up the project for local development.

Structure the response using the following subsections:

## Prerequisites
List required tools, SDKs, and accounts.

## Backend Setup
Provide step-by-step instructions to run the backend locally.

## Frontend Setup
Provide step-by-step instructions to run the frontend locally.

## Environment Configuration
Explain required environment variables and configuration setup.

## First Run & Verification
Describe how to run the application and verify everything is working correctly.

Formatting Requirements:
- Clearly separate explanations from commands.
- Use short explanatory text, followed by code blocks for commands.
- ALL commands, scripts, and configuration examples MUST be inside triple backticks code blocks.
- Use appropriate code block types (e.g., ```bash, ```powershell, ```json, ```env).
- Avoid long paragraphs—keep explanations concise and easy to scan.
- Use numbered steps for procedures.
- ALWAYS use triple backticks (```) to wrap code blocks.

Example style (follow this format strictly):

**Step 1: Install dependencies**

```bash
npm install
```

**Step 2: Start the development server**

```bash
npm start
```
""";

}