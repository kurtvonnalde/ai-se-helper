using Scalar.AspNetCore;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Interfaces;
using WebApi.Services;

//AI Related
using WebApi.AI.Agents;
using WebApi.AI.Interfaces;
using WebApi.AI.Services;
using WebApi.Middleware;
using WebApi.Options;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//controller
builder.Services.AddControllers();

//data
//builder.Services.AddDbContext<AppDbContext>(options =>
//options.UseInMemoryDatabase("AiPlanningDb"));

//sql server
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString, sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(10),
            errorNumbersToAdd: null);
        sqlOptions.CommandTimeout(60);
    }));


//services
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IInterviewService, InterviewService>();
builder.Services.AddScoped<IArtifactGenerationService, ArtifactGenerationService>();
builder.Services.Configure<AzureOpenAiOptions>(
    builder.Configuration.GetSection(AzureOpenAiOptions.SectionName));

builder.Services.AddSingleton<IChatClientFactory, AzureOpenAiChatClientFactory>();
builder.Services.AddScoped<IPromptBuilder, PromptBuilder>();
builder.Services.AddScoped<IArtifactAgent, ProjectBriefAgent>();
builder.Services.AddScoped<IArtifactAgent, SuggestedTechStackAgent>();
builder.Services.AddScoped<IArtifactAgent, SetupGuideAgent>();
builder.Services.AddScoped<IArtifactAgent, UserStoriesAgent>();
builder.Services.AddScoped<IArtifactAgent, CodebaseSummaryAgent>();
builder.Services.AddScoped<IArtifactAgent, ProjectArchitectureAgent>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .SetIsOriginAllowed(origin =>
            {
                if (!Uri.TryCreate(origin, UriKind.Absolute, out var uri))
                {
                    return false;
                }

                return uri.Scheme == Uri.UriSchemeHttp &&
                       (uri.Host == "localhost" || uri.Host == "127.0.0.1");
            })
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseCors("AllowFrontend");
app.UseAuthorization();
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.MapControllers();

app.Run();