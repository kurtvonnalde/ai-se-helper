using WebApi.DTOs.Artifacts;
using WebApi.Interfaces;
using WebApi.Data;
using WebApi.Entities;
using WebApi.Entities.Enum;
using WebApi.AI.Interfaces;
using WebApi.AI.Models;
using WebApi.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Services;


public class ArtifactGenerationService(
    AppDbContext dbContext,
    IEnumerable<IArtifactAgent> artifactAgents) : IArtifactGenerationService
{
    private static readonly string[] ArtifactOrder =
    [
        ArtifactTypes.ProjectBrief,
        ArtifactTypes.SuggestedTechStack,
        ArtifactTypes.SetupGuide,
        ArtifactTypes.UserStories,
        ArtifactTypes.CodebaseSummary,
        ArtifactTypes.ProjectArchitecture,
    ];

    public async Task<GenerateArtifactsResponse> GenerateAsync(Guid projectId, CancellationToken cancellationToken = default)
    {
        var project = await dbContext.Projects.FirstOrDefaultAsync(x => x.Id == projectId, cancellationToken)
                      ?? throw new NotFoundException("Project not found.");

        var session = await dbContext.InterviewSessions
            .Where(x => x.ProjectId == projectId)
            .OrderByDescending(x => x.StartedAtUtc)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException("Interview session not found.");

        var answers = await dbContext.InterviewAnswers
            .Where(x => x.SessionId == session.Id)
            .ToListAsync(cancellationToken);

        var answerMap = answers.ToDictionary(x => x.QuestionKey, x => x.AnswerText, StringComparer.OrdinalIgnoreCase);

        string Get(string key) => answerMap.TryGetValue(key, out var value) ? value : "Not provided";
        var planningContext = new ProjectPlanningContext
        {
            ProjectId = project.Id,
            ProjectTitle = project.Name,
            ProjectDescription = project.Description,
            ProblemStatement = Get("problem_statement"),
            TargetUsers = Get("target_users"),
            AppType = Get("app_type"),
            CoreFeatures = Get("core_features"),
            Authentication = Get("authentication"),
            AdminPanel = Get("admin_panel"),
            ReportsDashboard = Get("reports_dashboard"),
            Integrations = Get("integrations"),
            PreferredStack = Get("preferred_stack"),
            DeploymentPreference = Get("deployment_preference")
        };

        var orderedAgents = artifactAgents
            .OrderBy(agent => Array.IndexOf(ArtifactOrder, agent.ArtifactType))
            .ToList();

        if (orderedAgents.Count == 0)
        {
            throw new ExternalServiceException("No artifact agents are registered.", "artifact_agent_configuration_missing");
        }

        var artifacts = new List<ArtifactItemResponse>(orderedAgents.Count);

        foreach (var agent in orderedAgents)
        {
            var content = await agent.GenerateAsync(planningContext, cancellationToken);

            artifacts.Add(new ArtifactItemResponse
            {
                ArtifactType = agent.ArtifactType,
                ContentMarkdown = content
            });
        }

        var artifactEntities = new List<GeneratedArtifact>(artifacts.Count);

        foreach (var artifact in artifacts)
        {
            var nextVersion = await dbContext.GeneratedArtifacts
                .Where(x => x.ProjectId == projectId && x.ArtifactType == artifact.ArtifactType)
                .Select(x => (int?)x.Version)
                .MaxAsync(cancellationToken) ?? 0;

            artifactEntities.Add(new GeneratedArtifact
            {
                ProjectId = projectId,
                ArtifactType = artifact.ArtifactType,
                ContentMarkdown = artifact.ContentMarkdown,
                Version = nextVersion + 1,
                CreatedAtUtc = DateTime.UtcNow
            });
        }

        dbContext.GeneratedArtifacts.AddRange(artifactEntities);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new GenerateArtifactsResponse
        {
            ProjectId = projectId,
            Artifacts = artifacts
        };
    }

    public async Task<GenerateArtifactsResponse> GetAsync(Guid projectId, CancellationToken cancellationToken = default)
    {
        var projectExists = await dbContext.Projects
            .AnyAsync(x => x.Id == projectId, cancellationToken);

        if (!projectExists)
        {
            throw new NotFoundException("Project not found.");
        }

        var savedArtifacts = await dbContext.GeneratedArtifacts
            .Where(x => x.ProjectId == projectId)
            .OrderByDescending(x => x.Version)
            .ThenByDescending(x => x.CreatedAtUtc)
            .ToListAsync(cancellationToken);

        var latestByType = savedArtifacts
            .GroupBy(x => x.ArtifactType, StringComparer.OrdinalIgnoreCase)
            .Select(x => x.First())
            .OrderBy(x => Array.IndexOf(ArtifactOrder, x.ArtifactType))
            .ThenBy(x => x.ArtifactType, StringComparer.OrdinalIgnoreCase)
            .Select(x => new ArtifactItemResponse
            {
                ArtifactType = x.ArtifactType,
                ContentMarkdown = x.ContentMarkdown
            })
            .ToList();

        return new GenerateArtifactsResponse
        {
            ProjectId = projectId,
            Artifacts = latestByType
        };
    }
}
