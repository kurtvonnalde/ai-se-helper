using WebApi.AI.Models;

namespace WebApi.AI.Interfaces;

public interface IArtifactAgent
{
    string ArtifactType { get; }
    Task<string> GenerateAsync(ProjectPlanningContext context, CancellationToken cancellationToken = default);
}
