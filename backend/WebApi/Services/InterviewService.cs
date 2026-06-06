using WebApi.DTOs.Interview;
using WebApi.Interfaces;
using WebApi.Questions;
using WebApi.Entities;
using WebApi.Data;
using WebApi.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Services;


public class InterviewService(AppDbContext dbContext) : IInterviewService
{
    public async Task<StartInterviewResponse> StartAsync(Guid projectId, CancellationToken cancellationToken = default)
    {
        var projectExists = await dbContext.Projects.AnyAsync(x => x.Id == projectId, cancellationToken);
        if (!projectExists)
            throw new NotFoundException("Project not found.");

        var existingActiveSession = await dbContext.InterviewSessions
            .Where(x => x.ProjectId == projectId && !x.IsCompleted)
            .OrderByDescending(x => x.StartedAtUtc)
            .FirstOrDefaultAsync(cancellationToken);

        if (existingActiveSession is not null)
        {
            var existingQuestion = PlanningQuestionCatalog.GetByOrder(existingActiveSession.CurrentQuestionOrder)
                                  ?? throw new NotFoundException("Current question not found.");

            return new StartInterviewResponse
            {
                SessionId = existingActiveSession.Id,
                CurrentQuestionOrder = existingQuestion.Order,
                QuestionKey = existingQuestion.Key,
                QuestionText = existingQuestion.Text
            };
        }

        var firstQuestion = PlanningQuestionCatalog.GetByOrder(1)
                           ?? throw new ValidationException("First question not configured.");

        var session = new InterviewSession
        {
            ProjectId = projectId,
            CurrentQuestionOrder = firstQuestion.Order,
            IsCompleted = false
        };

        dbContext.InterviewSessions.Add(session);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new StartInterviewResponse
        {
            SessionId = session.Id,
            CurrentQuestionOrder = firstQuestion.Order,
            QuestionKey = firstQuestion.Key,
            QuestionText = firstQuestion.Text
        };
    }

    public async Task<CurrentQuestionResponse?> GetCurrentQuestionAsync(Guid projectId, CancellationToken cancellationToken = default)
    {
        var session = await dbContext.InterviewSessions
            .Where(x => x.ProjectId == projectId && !x.IsCompleted)
            .OrderByDescending(x => x.StartedAtUtc)
            .FirstOrDefaultAsync(cancellationToken);

        if (session is null)
            return null;

        var question = PlanningQuestionCatalog.GetByOrder(session.CurrentQuestionOrder);
        if (question is null)
        {
            return new CurrentQuestionResponse
            {
                SessionId = session.Id,
                CurrentQuestionOrder = session.CurrentQuestionOrder,
                SessionCompleted = session.IsCompleted
            };
        }

        return new CurrentQuestionResponse
        {
            SessionId = session.Id,
            CurrentQuestionOrder = question.Order,
            QuestionKey = question.Key,
            QuestionText = question.Text,
            SessionCompleted = session.IsCompleted
        };
    }

    public async Task<SubmitAnswerResponse> SubmitAnswerAsync(Guid projectId, SubmitAnswerRequest request, CancellationToken cancellationToken = default)
    {
        var session = await dbContext.InterviewSessions
            .FirstOrDefaultAsync(x => x.Id == request.SessionId && x.ProjectId == projectId, cancellationToken);

        if (session is null)
            throw new NotFoundException("Session not found.");

        if (session.IsCompleted)
            throw new ConflictException("Session is already completed.");

        var currentQuestion = PlanningQuestionCatalog.GetByKey(request.QuestionKey);
        if (currentQuestion is null)
            throw new ValidationException("Invalid question key.");

        if (string.IsNullOrWhiteSpace(request.AnswerText))
            throw new ValidationException("Answer text is required.");

        var alreadyAnswered = await dbContext.InterviewAnswers.AnyAsync(
            x => x.SessionId == session.Id && x.QuestionKey == request.QuestionKey,
            cancellationToken);

        if (alreadyAnswered)
            throw new ConflictException("This question has already been answered.");

        var answer = new InterviewAnswer
        {
            SessionId = session.Id,
            QuestionKey = currentQuestion.Key,
            QuestionText = currentQuestion.Text,
            AnswerText = request.AnswerText.Trim()
        };

        dbContext.InterviewAnswers.Add(answer);

        var nextQuestion = PlanningQuestionCatalog.GetByOrder(currentQuestion.Order + 1);

        if (nextQuestion is null)
        {
            session.IsCompleted = true;
            session.CompletedAtUtc = DateTime.UtcNow;
            await dbContext.SaveChangesAsync(cancellationToken);

            return new SubmitAnswerResponse
            {
                SessionCompleted = true
            };
        }

        session.CurrentQuestionOrder = nextQuestion.Order;
        await dbContext.SaveChangesAsync(cancellationToken);

        return new SubmitAnswerResponse
        {
            SessionCompleted = false,
            NextQuestionOrder = nextQuestion.Order,
            NextQuestionKey = nextQuestion.Key,
            NextQuestionText = nextQuestion.Text
        };
    }

    public async Task<List<InterviewAnswerResponse>> GetAnswersAsync(Guid projectId, CancellationToken cancellationToken = default)
    {
        var session = await dbContext.InterviewSessions
            .Where(x => x.ProjectId == projectId)
            .OrderByDescending(x => x.StartedAtUtc)
            .FirstOrDefaultAsync(cancellationToken);

        if (session is null)
            return new List<InterviewAnswerResponse>();

        return await dbContext.InterviewAnswers
            .Where(x => x.SessionId == session.Id)
            .OrderBy(x => x.CreatedAtUtc)
            .Select(x => new InterviewAnswerResponse
            {
                QuestionKey = x.QuestionKey,
                QuestionText = x.QuestionText,
                AnswerText = x.AnswerText,
                CreatedAtUtc = x.CreatedAtUtc
            })
            .ToListAsync(cancellationToken);
    }
}
