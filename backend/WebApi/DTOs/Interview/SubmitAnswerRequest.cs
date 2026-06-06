namespace WebApi.DTOs.Interview;

public class SubmitAnswerRequest
{
    public Guid SessionId { get; set; }
    public string QuestionKey { get; set; } = string.Empty;
    public string AnswerText { get; set; } = string.Empty;
}