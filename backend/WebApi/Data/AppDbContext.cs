using Microsoft.EntityFrameworkCore;
using WebApi.Entities;

namespace WebApi.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<InterviewSession> InterviewSessions => Set<InterviewSession>();
    public DbSet<InterviewAnswer> InterviewAnswers => Set<InterviewAnswer>();
    public DbSet<GeneratedArtifact> GeneratedArtifacts => Set<GeneratedArtifact>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(x => x.Description)
                .HasMaxLength(2000);

            entity.Property(x => x.CreatedAtUtc)
                .IsRequired();

            entity.Property(x => x.UpdatedAtUtc)
                .IsRequired();

            entity.HasMany(x => x.InterviewSessions)
                .WithOne(x => x.Project)
                .HasForeignKey(x => x.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(x => x.GeneratedArtifacts)
                .WithOne(x => x.Project)
                .HasForeignKey(x => x.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<InterviewSession>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.ProjectId)
                .IsRequired();

            entity.Property(x => x.CurrentQuestionOrder)
                .IsRequired();

            entity.Property(x => x.IsCompleted)
                .IsRequired();

            entity.Property(x => x.StartedAtUtc)
                .IsRequired();

            entity.Property(x => x.CompletedAtUtc);

            entity.HasMany(x => x.Answers)
                .WithOne(x => x.Session)
                .HasForeignKey(x => x.SessionId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<InterviewAnswer>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.SessionId)
                .IsRequired();

            entity.Property(x => x.QuestionKey)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(x => x.QuestionText)
                .IsRequired()
                .HasMaxLength(1000);

            entity.Property(x => x.AnswerText)
                .IsRequired()
                .HasMaxLength(4000);

            entity.Property(x => x.CreatedAtUtc)
                .IsRequired();
        });

        modelBuilder.Entity<GeneratedArtifact>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.ProjectId)
                .IsRequired();

            entity.Property(x => x.ArtifactType)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(x => x.ContentMarkdown)
                .IsRequired();

            entity.Property(x => x.Version)
                .IsRequired();

            entity.Property(x => x.CreatedAtUtc)
                .IsRequired();
        });
    }
}