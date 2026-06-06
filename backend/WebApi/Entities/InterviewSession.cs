namespace WebApi.Entities;

public class InterviewSession
{

    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ProjectId { get; set; }
    public int CurrentQuestionOrder { get; set; } = 1;
    public bool IsCompleted { get; set; } = false;
    public DateTime StartedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAtUtc { get; set; }

    public Project? Project { get; set; }
    public ICollection<InterviewAnswer> Answers { get; set; } = new List<InterviewAnswer>();

}