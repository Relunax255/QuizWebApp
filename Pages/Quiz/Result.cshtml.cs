using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using QuizWebApp.Helpers;
using QuizWebApp.Models;

namespace QuizWebApp.Pages
{
    public class ResultModel : PageModel
    {
        private readonly QuizDbContext db;
        public ResultModel(QuizDbContext db) => this.db = db;

        public List<QuestionResultView> Results { get; set; } = new();
        public int Score { get; set; }
        public int Total { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = HttpContext.GetOrCreateUserId();
            var session = await db.QuizSessions.FirstOrDefaultAsync(s => s.UserId == userId);
            if (session == null) return RedirectToPage("QuizStart");

            var quizObj = JsonConvert.DeserializeObject<Models.Quiz>(session.QuizJson);
            var resultsList = JsonConvert.DeserializeObject<List<QuestionResult>>(session.ResultsJson);

            Total = quizObj.Questions.Count;
            Score = resultsList.Count(r => r.IsCorrect);

            foreach (var r in resultsList)
            {
                var q = quizObj.Questions.First(x => x.QuestionId == r.QuestionId);
                var selected = q.Answers.First(a => a.AnswerId == r.SelectedAnswerId);
                var correct = q.Answers.First(a => a.IsCorrect);

                Results.Add(new QuestionResultView
                {
                    Question = q,
                    SelectedAnswer = selected,
                    CorrectAnswer = correct,
                    IsCorrect = r.IsCorrect
                });
            }

            var completedQuiz = new CompletedQuiz
            {
                Date = DateTime.UtcNow,
                UserId = userId,
                Questions = Results.Select(r => new CompletedQuestion
                {
                    QuestionText = r.Question.QuestionText,
                    Answers = r.Question.Answers.Select(a => new CompletedAnswer
                    {
                        AnswerText = a.AnswerText,
                        IsCorrect = a.IsCorrect,
                        IsSelected = a.AnswerId == r.SelectedAnswer.AnswerId
                    }).ToList()
                }).ToList()
            };

            db.CompletedQuizzes.Add(completedQuiz);
            db.QuizSessions.Remove(session);
            await db.SaveChangesAsync();

            return Page();
        }
    }
    public class QuestionResultView
    {
        public Question Question { get; set; }
        public Answer SelectedAnswer { get; set; }
        public Answer CorrectAnswer { get; set; }
        public bool IsCorrect { get; set; }
    }
}