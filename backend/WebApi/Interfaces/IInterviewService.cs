using WebApi.DTOs.Interview;

namespace WebApi.Interfaces;

public interface IInterviewService
{
    Task<StartInterviewResponse> StartAsync(Guid projectId, CancellationToken cancellationToken = default);
    Task<CurrentQuestionResponse?> GetCurrentQuestionAsync(Guid projectId, CancellationToken cancellationToken = default);
    Task<SubmitAnswerResponse> SubmitAnswerAsync(Guid projectId, SubmitAnswerRequest request, CancellationToken cancellationToken = default);
    Task<List<InterviewAnswerResponse>> GetAnswersAsync(Guid projectId, CancellationToken cancellationToken = default);
}