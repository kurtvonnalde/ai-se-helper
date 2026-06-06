using WebApi.DTOs.Projects;
using WebApi.Interfaces;
using WebApi.Entities;
using WebApi.Data;
using WebApi.Exceptions;
using Microsoft.EntityFrameworkCore;


namespace WebApi.Services;


public class ProjectService(AppDbContext dbContext) : IProjectService
{
    public async Task<ProjectResponse> CreateAsync(CreateProjectRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
            throw new ValidationException("Project title is required.");

        if (string.IsNullOrWhiteSpace(request.Description))
            throw new ValidationException("Project description is required.");

        var entity = new Project
        {
            Name = request.Title.Trim(),
            Description = request.Description.Trim()
        };

        dbContext.Projects.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new ProjectResponse
        {
            Id = entity.Id,
            Title = entity.Name,
            Description = entity.Description,
            CreatedAtUtc = entity.CreatedAtUtc
        };
    }

    public async Task<List<ProjectResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Projects
            .OrderByDescending(x => x.CreatedAtUtc)
            .Select(x => new ProjectResponse
            {
                Id = x.Id,
                Title = x.Name,
                Description = x.Description,
                CreatedAtUtc = x.CreatedAtUtc
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<ProjectResponse?> GetByIdAsync(Guid projectId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Projects
            .Where(x => x.Id == projectId)
            .Select(x => new ProjectResponse
            {
                Id = x.Id,
                Title = x.Name,
                Description = x.Description,
                CreatedAtUtc = x.CreatedAtUtc
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}

