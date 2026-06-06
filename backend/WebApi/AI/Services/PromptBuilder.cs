using WebApi.AI.Interfaces;
using WebApi.AI.Models;

namespace WebApi.AI.Services;

public class PromptBuilder : IPromptBuilder
{
    public string BuildContextPrompt(ProjectPlanningContext context)
    {
        return $"""
        Project Title: {context.ProjectTitle}
        Project Description: {context.ProjectDescription}
        Problem Statement: {context.ProblemStatement}
        Target Users: {context.TargetUsers}
        App Type: {context.AppType}
        Core Features: {context.CoreFeatures}
        Authentication: {context.Authentication}
        Admin Panel: {context.AdminPanel}
        Reports / Dashboard: {context.ReportsDashboard}
        Integrations: {context.Integrations}
        Preferred Stack: {context.PreferredStack}
        Deployment Preference: {context.DeploymentPreference}
        """;
    }
}