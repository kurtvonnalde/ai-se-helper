namespace WebApi.DTOs.Interview;

public class InterviewAnswerResponse
{
    public string QuestionKey { get; set; } = string.Empty;
    public string QuestionText { get; set; } = string.Empty;
    public string AnswerText { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; }
}