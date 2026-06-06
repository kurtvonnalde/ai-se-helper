namespace WebApi.DTOs.Interview;

public class CurrentQuestionResponse
{
    public Guid SessionId { get; set; }
    public int CurrentQuestionOrder { get; set; }
    public string QuestionKey { get; set; } = string.Empty;
    public string QuestionText { get; set; } = string.Empty;
    public bool SessionCompleted { get; set; }
}