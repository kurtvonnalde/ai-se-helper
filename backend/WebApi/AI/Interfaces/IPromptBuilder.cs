using WebApi.AI.Models;

namespace WebApi.AI.Interfaces;

public interface IPromptBuilder
{
    string BuildContextPrompt(ProjectPlanningContext context);
}
