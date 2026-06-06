using WebApi.DTOs.Projects;

namespace WebApi.Interfaces;

public interface IProjectService
{
    Task<ProjectResponse> CreateAsync(CreateProjectRequest request, CancellationToken cancellationToken = default);
    Task<List<ProjectResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ProjectResponse?> GetByIdAsync(Guid projectId, CancellationToken cancellationToken = default);
}
