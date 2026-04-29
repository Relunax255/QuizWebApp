using Microsoft.EntityFrameworkCore;
using QuizWebApp.Models;
namespace QuizWebApp.Data
{
    public class QuizDbContext : DbContext
    {
        public QuizDbContext(DbContextOptions options) : base(options) { }

        public DbSet<CompletedQuiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<CompletedQuiz>().HasMany(q => q.Questions);
            modelBuilder.Entity<Question>().HasMany(q => q.Answers).WithOne(a => a.Question).HasForeignKey(a => a.QuestionId);
            modelBuilder.Entity<Category>().HasMany(c => c.Questions);
        }
    }
}
