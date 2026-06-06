using System.Text.RegularExpressions;
using OpenAI.Chat;
using WebApi.AI.Interfaces;
using WebApi.AI.Models;

namespace WebApi.AI.Agents;

public abstract class PromptArtifactAgentBase(
    IChatClientFactory chatClientFactory,
    IPromptBuilder promptBuilder) : IArtifactAgent
{
    public abstract string ArtifactType { get; }

    protected abstract string SystemPrompt { get; }

    public async Task<string> GenerateAsync(
        ProjectPlanningContext context,
        CancellationToken cancellationToken = default)
    {
        var chatClient = chatClientFactory.Create();
        var prompt = promptBuilder.BuildContextPrompt(context);

        var result = await chatClient.CompleteChatAsync(
            [
                new SystemChatMessage(SystemPrompt),
                new UserChatMessage(prompt)
            ],
            cancellationToken: cancellationToken);

        var rawContent = string.Join(
            "",
            result.Value.Content
                .Where(contentPart => !string.IsNullOrWhiteSpace(contentPart.Text))
                .Select(contentPart => contentPart.Text));

        return NormalizeCodeBlocks(rawContent);
    }

    private static string NormalizeCodeBlocks(string content)
    {
        // Fix code blocks: detect standalone commands (npm, dotnet, powershell, bash, etc.)
        // that appear after colons or as list items and wrap them if not already wrapped
        var lines = content.Split('\n');
        var result = new List<string>();
        var inCodeBlock = false;

        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i];

            // Check if we're entering or exiting a code block
            if (line.TrimStart().StartsWith("```"))
            {
                inCodeBlock = !inCodeBlock;
                result.Add(line);
                continue;
            }

            if (inCodeBlock)
            {
                result.Add(line);
                continue;
            }

            // Detect standalone commands that should be code blocks
            var trimmed = line.TrimStart();
            if (trimmed.Length > 0 && !trimmed.StartsWith("#") && !trimmed.StartsWith(">") &&
                IsStandaloneCommand(trimmed))
            {
                // This is a command that should be in a code block
                result.Add("```bash");
                result.Add(line);
                
                // Consume following command lines until we hit a non-command
                while (i + 1 < lines.Length)
                {
                    var nextLine = lines[i + 1];
                    var nextTrimmed = nextLine.TrimStart();
                    
                    if (nextTrimmed.Length == 0 || nextTrimmed.StartsWith("#") || 
                        nextTrimmed.StartsWith(">") || nextTrimmed.StartsWith("```") ||
                        (!IsStandaloneCommand(nextTrimmed) && !IsContinuationLine(nextTrimmed)))
                    {
                        break;
                    }
                    
                    i++;
                    result.Add(lines[i]);
                }
                
                result.Add("```");
            }
            else
            {
                result.Add(line);
            }
        }

        return string.Join("\n", result);
    }

    private static bool IsStandaloneCommand(string line)
    {
        // Check if line looks like a command (starts with common command patterns)
        var commandPatterns = new[]
        {
            @"^(npm|yarn|dotnet|python|pip|powershell|pwsh|bash|sh|cd|mkdir|git|docker|kubectl|mongod|mongo|node|ng|tsc)",
            @"^(Set-Location|Get-|New-|Remove-|Install-|Invoke-)", // PowerShell
            @"^[a-zA-Z0-9_\-]+\s+(install|start|run|build|dev|test|serve)" // Common command patterns
        };

        return commandPatterns.Any(pattern => Regex.IsMatch(line, pattern, RegexOptions.IgnoreCase));
    }

    private static bool IsContinuationLine(string line)
    {
        // Lines that continue a command (pipes, redirects, continuations)
        return Regex.IsMatch(line, @"^(\s*\||\s*>|\s*&&|\s*;|\s*\$)");
    }
}