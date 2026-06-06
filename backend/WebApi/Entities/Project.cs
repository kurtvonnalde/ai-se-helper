namespace WebApi.Entities;
public class Project
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;

    
    public ICollection<InterviewSession> InterviewSessions { get; set; } = new List<InterviewSession>();
    public ICollection<GeneratedArtifact> GeneratedArtifacts { get; set; } = new List<GeneratedArtifact>();

}