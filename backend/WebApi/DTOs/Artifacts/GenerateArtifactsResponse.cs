namespace WebApi.DTOs.Artifacts;

public class GenerateArtifactsResponse
{
    public Guid ProjectId { get; set; }
    public List<ArtifactItemResponse> Artifacts { get; set; } = new();
}
