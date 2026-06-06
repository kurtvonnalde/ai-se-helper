namespace WebApi.AI.Models;

public class ProjectPlanningContext

{
    public Guid ProjectId { get; set; }
    public string ProjectTitle { get; set; } = string.Empty;
    public string ProjectDescription { get; set; } = string.Empty;
    public string ProblemStatement { get; set; } = string.Empty;
    public string TargetUsers { get; set; } = string.Empty;
    public string AppType { get; set; } = string.Empty;
    public string CoreFeatures { get; set; } = string.Empty;
    public string Authentication { get; set; } = string.Empty;
    public string AdminPanel { get; set; } = string.Empty;
    public string ReportsDashboard { get; set; } = string.Empty;
    public string Integrations { get; set; } = string.Empty;
    public string PreferredStack { get; set; } = string.Empty;
    public string DeploymentPreference { get; set; } = string.Empty;
}
