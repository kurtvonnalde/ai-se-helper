using WebApi.DTOs.Artifacts;

namespace WebApi.Interfaces;

public interface IArtifactGenerationService
{
    Task<GenerateArtifactsResponse> GenerateAsync(Guid projectId, CancellationToken cancellationToken = default);
}