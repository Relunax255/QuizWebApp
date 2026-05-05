using Microsoft.EntityFrameworkCore;
using QuizWebApp.Models;

public class QuizDbContext : DbContext
{
    public QuizDbContext(DbContextOptions options) : base(options) { }

    public DbSet<Category> Categories { get; set; }

    public DbSet<CompletedQuiz> CompletedQuizzes { get; set; }
    public DbSet<CompletedQuestion> CompletedQuestions { get; set; }
    public DbSet<CompletedAnswer> CompletedAnswers { get; set; }
    public DbSet<QuizSession> QuizSessions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<CompletedQuiz>()
        .HasMany(q => q.Questions)
        .WithOne(q => q.CompletedQuiz)
        .HasForeignKey(q => q.CompletedQuizId)
        .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CompletedQuestion>()
            .HasMany(q => q.Answers)
            .WithOne(a => a.CompletedQuestion)
            .HasForeignKey(a => a.CompletedQuestionId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<QuizSession>()
            .Property(q => q.QuizJson)
            .IsRequired();
        modelBuilder.Entity<QuizSession>()
            .Property(q => q.ResultsJson)
            .IsRequired();
    }
}