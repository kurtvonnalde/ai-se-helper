namespace WebApi.Entities;

public class GeneratedArtifact
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ProjectId { get; set; }

    public string ArtifactType { get; set; } = string.Empty;
    public string ContentMarkdown { get; set; } = string.Empty;
    public int Version { get; set; } = 1;

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public Project? Project { get; set; }

}
