using WebApi.DTOs.Artifacts;
using WebApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;



[ApiController]
[Route("api/projects/{projectId:guid}/artifacts")]
public class ArtifactsController(IArtifactGenerationService artifactGenerationService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<GenerateArtifactsResponse>> Get(
        Guid projectId,
        CancellationToken cancellationToken)
    {
        var result = await artifactGenerationService.GetAsync(projectId, cancellationToken);
        return Ok(result);
    }

    [HttpPost("generate")]
    public async Task<ActionResult<GenerateArtifactsResponse>> Generate(
        Guid projectId,
        CancellationToken cancellationToken)
    {
        var result = await artifactGenerationService.GenerateAsync(projectId, cancellationToken);
        return Ok(result);
    }
}

