namespace WebApi.DTOs.Interview;

public class StartInterviewResponse
{
    public Guid SessionId { get; set; }
    public int CurrentQuestionOrder { get; set; }
    public string QuestionKey { get; set; } = string.Empty;
    public string QuestionText { get; set; } = string.Empty;
}
