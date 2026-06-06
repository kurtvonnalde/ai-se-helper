using WebApi.DTOs.Interview;
using WebApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;


[ApiController]
[Route("api/projects/{projectId:guid}/interview")]
public class InterviewController(IInterviewService interviewService) : ControllerBase
{
    [HttpPost("start")]
    public async Task<ActionResult<StartInterviewResponse>> Start(Guid projectId, CancellationToken cancellationToken)
    {
        var result = await interviewService.StartAsync(projectId, cancellationToken);
        return Ok(result);
    }

    [HttpGet("current")]
    public async Task<ActionResult<CurrentQuestionResponse>> GetCurrent(Guid projectId, CancellationToken cancellationToken)
    {
        var result = await interviewService.GetCurrentQuestionAsync(projectId, cancellationToken);
        if (result is null)
            return NotFound(new { message = "No active session found." });

        return Ok(result);
    }

    [HttpPost("answer")]
    public async Task<ActionResult<SubmitAnswerResponse>> SubmitAnswer(
        Guid projectId,
        [FromBody] SubmitAnswerRequest request,
        CancellationToken cancellationToken)
    {
        var result = await interviewService.SubmitAnswerAsync(projectId, request, cancellationToken);
        return Ok(result);
    }

    [HttpGet("answers")]
    public async Task<ActionResult<List<InterviewAnswerResponse>>> GetAnswers(Guid projectId, CancellationToken cancellationToken)
    {
        var result = await interviewService.GetAnswersAsync(projectId, cancellationToken);
        return Ok(result);
    }
}
