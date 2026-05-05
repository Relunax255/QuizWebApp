using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using QuizWebApp.Models;
using QuizWebApp.Helpers;
namespace QuizWebApp.Pages
{
    public class QuizGameModel : PageModel
    {
        private readonly QuizDbContext db;
        public QuizGameModel(QuizDbContext db) => this.db = db;

        public Models.Quiz QuizObj { get; set; }
        public Question CurrentQuestion { get; set; }
        public int Index { get; set; }
        public int CurrentNumber => Index + 1;
        public int TotalQuestions => QuizObj?.Questions.Count ?? 0;

        [BindProperty]
        public int SelectedAnswerId { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var session = await GetSessionAsync();
            if (session == null) return RedirectToPage("QuizStart");

            LoadQuiz(session);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var session = await GetSessionAsync();
            if (session == null) return RedirectToPage("QuizStart");

            LoadQuiz(session);

            var results = JsonConvert.DeserializeObject<List<QuestionResult>>(session.ResultsJson);

            results.Add(new QuestionResult
            {
                QuestionId = CurrentQuestion.QuestionId,
                SelectedAnswerId = SelectedAnswerId,
                IsCorrect = CurrentQuestion.Answers.First(a => a.AnswerId == SelectedAnswerId).IsCorrect
            });

            session.ResultsJson = JsonConvert.SerializeObject(results);
            session.CurrentQuestionIndex++;
            await db.SaveChangesAsync();

            if (session.CurrentQuestionIndex >= QuizObj.Questions.Count)
                return RedirectToPage("Result");

            return RedirectToPage();
        }

        private async Task<QuizSession> GetSessionAsync()
        {
            var userId = HttpContext.GetOrCreateUserId();
            return await db.QuizSessions.FirstOrDefaultAsync(s => s.UserId == userId);
        }

        private void LoadQuiz(QuizSession session)
        {
            QuizObj = JsonConvert.DeserializeObject<Models.Quiz>(session.QuizJson);
            Index = session.CurrentQuestionIndex;
            if (Index < QuizObj.Questions.Count)
                CurrentQuestion = QuizObj.Questions[Index];
        }
    }
}