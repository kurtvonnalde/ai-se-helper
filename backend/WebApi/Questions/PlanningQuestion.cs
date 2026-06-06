namespace WebApi.Questions;

public class PlanningQuestion
{
    public int Order { get; set; }
    public string Key { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public bool Required { get; set; }
}