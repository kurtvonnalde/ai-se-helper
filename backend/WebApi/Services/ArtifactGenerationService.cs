using WebApi.DTOs.Artifacts;
using WebApi.Interfaces;
using WebApi.Data;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Services;


public class ArtifactGenerationService(AppDbContext dbContext) : IArtifactGenerationService
{
    public async Task<GenerateArtifactsResponse> GenerateAsync(Guid projectId, CancellationToken cancellationToken = default)
    {
        var project = await dbContext.Projects.FirstOrDefaultAsync(x => x.Id == projectId, cancellationToken)
                      ?? throw new InvalidOperationException("Project not found.");

        var session = await dbContext.InterviewSessions
            .Where(x => x.ProjectId == projectId)
            .OrderByDescending(x => x.StartedAtUtc)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new InvalidOperationException("Interview session not found.");

        var answers = await dbContext.InterviewAnswers
            .Where(x => x.SessionId == session.Id)
            .ToListAsync(cancellationToken);

        var answerMap = answers.ToDictionary(x => x.QuestionKey, x => x.AnswerText, StringComparer.OrdinalIgnoreCase);

        string Get(string key) => answerMap.TryGetValue(key, out var value) ? value : "Not provided";

        var projectBrief = $"""
        # Project Brief

        **Project Name:** {project.Name}

        **Description:** {project.Description}

        **Problem Statement:** {Get("problem_statement")}

        **Target Users:** {Get("target_users")}

        **Application Type:** {Get("app_type")}
        """;

        var techStack = $"""
        # Suggested Tech Stack

        - Frontend: React + TypeScript
        - Backend: ASP.NET Core Web API
        - API Documentation UI: Scalar
        - Database: SQL Server or PostgreSQL
        - Hosting: Azure App Service
        - AI Integration: Azure OpenAI
        """;

        var setupGuide = $"""
        # Setup Guide

        1. Clone the repository
        2. Run the backend API
        3. Run the React frontend
        4. Configure environment variables
        5. Connect backend and frontend API base URL
        6. Complete the interview workflow
        7. Generate planning artifacts
        """;

        var userStories = $"""
        # User Stories

        - As a planner, I want to answer guided software planning questions so that the system can understand my project idea.
        - As a planner, I want the system to summarize my idea so that I can review the proposed direction.
        - As a planner, I want suggested technologies so that I can quickly decide how to build the solution.
        - As a planner, I want a setup guide so that I can begin implementation faster.
        """;

        var artifacts = new List<ArtifactItemResponse>
        {
            new() { ArtifactType = "project-brief", ContentMarkdown = projectBrief },
            new() { ArtifactType = "suggested-tech-stack", ContentMarkdown = techStack },
            new() { ArtifactType = "setup-guide", ContentMarkdown = setupGuide },
            new() { ArtifactType = "user-stories", ContentMarkdown = userStories }
        };

        return new GenerateArtifactsResponse
        {
            ProjectId = projectId,
            Artifacts = artifacts
        };
    }
}
