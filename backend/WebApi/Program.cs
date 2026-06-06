using Scalar.AspNetCore;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Interfaces;
using WebApi.Services;

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
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


//services
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IInterviewService, InterviewService>();
builder.Services.AddScoped<IArtifactGenerationService, ArtifactGenerationService>();
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

app.UseCors("AllowFrontend");
app.UseAuthorization();
app.UseHttpsRedirection();

app.MapControllers();

app.Run();