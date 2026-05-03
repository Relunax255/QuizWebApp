using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QuizWebApp.Models;
using System.Security.Claims;
using QuizWebApp.Helpers;

namespace QuizWebApp.Pages
{
    public class QuizHistoryModel : PageModel
    {
        private readonly QuizDbContext dbcontext;

        public QuizHistoryModel(QuizDbContext db)
        {
            dbcontext = db;
        }

        public List<CompletedQuiz> Quizzes { get; set; } = new();

        public async Task OnGetAsync()
        {
            var userId = HttpContext.Session.GetOrCreateUserId();

            Quizzes = await dbcontext.CompletedQuizzes
                .Where(q => q.UserId == userId)
                .Include(q => q.Questions)
                    .ThenInclude(q => q.Answers)
                .OrderByDescending(q => q.Date)
                .ToListAsync();
        }

        public int GetScore(CompletedQuiz quiz)
        {
            return quiz.Questions.Count(q =>
                q.Answers.Any(a => a.IsCorrect && a.IsSelected));
        }

        public int GetTotal(CompletedQuiz quiz)
        {
            return quiz.Questions.Count;
        }
    }
}