namespace WebApi.DTOs.Artifacts;

public class GenerateArtifactsResponse
{
    public Guid ProjectId { get; set; }
    public List<ArtifactItemResponse> Artifacts { get; set; } = new();
}

public class ArtifactItemResponse
{
    public string ArtifactType { get; set; } = string.Empty;
    public string ContentMarkdown { get; set; } = string.Empty;
}
