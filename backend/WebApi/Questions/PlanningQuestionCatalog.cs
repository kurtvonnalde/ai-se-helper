namespace WebApi.Questions;

public static class PlanningQuestionCatalog
{
    public static readonly List<PlanningQuestion> All = new()
    {
        new() { Order = 1, Key = "problem_statement", Text = "What problem are you trying to solve?", Required = true },
        new() { Order = 2, Key = "target_users", Text = "Who are the target users of this system?", Required = true },
        new() { Order = 3, Key = "app_type", Text = "Is this a web app, mobile app, internal tool, or public platform?", Required = true },
        new() { Order = 4, Key = "core_features", Text = "What are the core features you want in the application?", Required = true },
        new() { Order = 5, Key = "authentication", Text = "Do users need login or authentication?", Required = true },
        new() { Order = 6, Key = "admin_panel", Text = "Do you need an admin panel or back-office features?", Required = false },
        new() { Order = 7, Key = "reports_dashboard", Text = "Do you need reports, charts, or dashboards?", Required = false },
        new() { Order = 8, Key = "integrations", Text = "Does the system need integrations with other systems or APIs?", Required = false },
        new() { Order = 9, Key = "preferred_stack", Text = "Do you already have a preferred technology stack?", Required = false },
        new() { Order = 10, Key = "deployment_preference", Text = "Where do you want to deploy this system?", Required = false }
    };

    public static PlanningQuestion? GetByOrder(int order)
        => All.FirstOrDefault(q => q.Order == order);

    public static PlanningQuestion? GetByKey(string key)
        => All.FirstOrDefault(q => q.Key.Equals(key, StringComparison.OrdinalIgnoreCase));

    public static int MaxOrder()
        => All.Max(q => q.Order);
}