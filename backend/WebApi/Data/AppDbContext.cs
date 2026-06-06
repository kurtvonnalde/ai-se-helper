using WebApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<InterviewSession> InterviewSessions => Set<InterviewSession>();
    public DbSet<InterviewAnswer> InterviewAnswers => Set<InterviewAnswer>();
    public DbSet<GeneratedArtifact> GeneratedArtifacts => Set<GeneratedArtifact>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).HasMaxLength(200).IsRequired();
            entity.Property(x => x.Description).HasMaxLength(2000);
        });

        modelBuilder.Entity<InterviewSession>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.HasOne(x => x.Project)
                  .WithMany(x => x.InterviewSessions)
                  .HasForeignKey(x => x.ProjectId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<InterviewAnswer>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.QuestionKey).HasMaxLength(100).IsRequired();
            entity.Property(x => x.QuestionText).HasMaxLength(1000).IsRequired();
            entity.Property(x => x.AnswerText).HasMaxLength(4000).IsRequired();

            entity.HasOne(x => x.Session)
                  .WithMany(x => x.Answers)
                  .HasForeignKey(x => x.SessionId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<GeneratedArtifact>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.ArtifactType).HasMaxLength(100).IsRequired();
            entity.Property(x => x.ContentMarkdown).IsRequired();

            entity.HasOne(x => x.Project)
                  .WithMany(x => x.GeneratedArtifacts)
                  .HasForeignKey(x => x.ProjectId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}