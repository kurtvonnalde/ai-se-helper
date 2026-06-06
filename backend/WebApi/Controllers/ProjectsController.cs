using WebApi.DTOs.Projects;
using WebApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/projects")]
public class ProjectsController(IProjectService projectService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ProjectResponse>> Create(
        [FromBody] CreateProjectRequest request,
        CancellationToken cancellationToken)
    {
        var result = await projectService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { projectId = result.Id }, result);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProjectResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await projectService.GetAllAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("{projectId:guid}")]
    public async Task<ActionResult<ProjectResponse>> GetById(Guid projectId, CancellationToken cancellationToken)
    {
        var result = await projectService.GetByIdAsync(projectId, cancellationToken);
        if (result is null)
            return NotFound();

        return Ok(result);
    }
}