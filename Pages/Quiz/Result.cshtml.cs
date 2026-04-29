using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuizWebApp.Models;
using System.Text.Json;

namespace QuizWebApp.Pages
{
    public class ResultModel : PageModel
    {
        public Models.Quiz Quiz { get; set; }
        public List<QuestionResultView> Results { get; set; } = new();

        public int Score { get; set; }
        public int Total { get; set; }

        public IActionResult OnGet()
        {
            var quizJson = HttpContext.Session.GetString("CurrentQuiz");
            var resultsJson = HttpContext.Session.GetString("QuizResults");

            if (quizJson == null || resultsJson == null)
                return RedirectToPage("Index");

            Quiz = JsonSerializer.Deserialize<Models.Quiz>(quizJson);
            var results = JsonSerializer.Deserialize<List<QuestionResult>>(resultsJson);

            Total = Quiz.Questions.Count;
            Score = results.Count(r => r.IsCorrect);

            foreach (var r in results)
            {
                var question = Quiz.Questions.First(q => q.QuestionId == r.QuestionId);

                var selected = question.Answers.First(a => a.AnswerId == r.SelectedAnswerId);
                var correct = question.Answers.First(a => a.IsCorrect);

                Results.Add(new QuestionResultView
                {
                    Question = question,
                    SelectedAnswer = selected,
                    CorrectAnswer = correct,
                    IsCorrect = r.IsCorrect
                });
            }
            HttpContext.Session.Remove("CurrentQuiz");
            HttpContext.Session.Remove("CurrentQuestionIndex");
            HttpContext.Session.Remove("QuizResults");

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