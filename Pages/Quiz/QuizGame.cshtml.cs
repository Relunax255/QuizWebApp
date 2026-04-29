using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuizWebApp.Models;
using Newtonsoft.Json;

namespace QuizWebApp.Pages
{
    public class QuizGameModel : PageModel
    {
        private const string QuizKey = "CurrentQuiz";
        private const string IndexKey = "CurrentQuestionIndex";
        private const string ResultsKey = "QuizResults";
        public Models.Quiz Quiz { get; set; }
        public Question CurrentQuestion { get; set; }

        public int Index { get; set; }
        public int IndexDisplay => Index + 1;

        [BindProperty]
        public int SelectedAnswerId { get; set; }

        public IActionResult OnGet()
        {
            return Load();
        }

        public int TotalQuestions => Quiz?.Questions?.Count ?? 0;
        public int CurrentNumber => Index + 1;

        public IActionResult OnPost()
        {
            var loadResult = Load();
            if (loadResult != null)
                return loadResult;

            var resultsJson = HttpContext.Session.GetString("QuizResults");
            var results = resultsJson == null
                ? new List<QuestionResult>()
                : JsonConvert.DeserializeObject<List<QuestionResult>>(resultsJson);

            var selectedAnswer = CurrentQuestion.Answers
                .FirstOrDefault(a => a.AnswerId == SelectedAnswerId);

            bool isCorrect = selectedAnswer?.IsCorrect == true;

            // store result per question
            results.Add(new QuestionResult
            {
                QuestionId = CurrentQuestion.QuestionId,
                SelectedAnswerId = SelectedAnswerId,
                IsCorrect = isCorrect
            });

            HttpContext.Session.SetString("QuizResults", JsonConvert.SerializeObject(results));

            // advance
            Index++;
            HttpContext.Session.SetInt32(IndexKey, Index);

            if (Index >= Quiz.Questions.Count)
            {
                return RedirectToPage("Result");
            }

            return RedirectToPage();
        }

        private IActionResult Load()
        {
            var json = HttpContext.Session.GetString("CurrentQuiz");

            if (json == null)
                return RedirectToPage("Index");

            Quiz = JsonConvert.DeserializeObject<Models.Quiz>(json);

            Index = HttpContext.Session.GetInt32("CurrentQuestionIndex") ?? 0;

            if (Index < Quiz.Questions.Count)
                CurrentQuestion = Quiz.Questions[Index];

            return null;
        }
    }
}