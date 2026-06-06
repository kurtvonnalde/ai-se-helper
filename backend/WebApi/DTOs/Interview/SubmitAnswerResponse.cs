namespace WebApi.DTOs.Interview;

public class SubmitAnswerResponse
{
    public bool SessionCompleted { get; set; }
    public int? NextQuestionOrder { get; set; }
    public string? NextQuestionKey { get; set; }
    public string? NextQuestionText { get; set; }
}