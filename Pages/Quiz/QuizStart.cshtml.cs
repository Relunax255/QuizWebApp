using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using QuizWebApp.Models;
using QuizWebApp.Services;
using System.Text.Json;

namespace QuizWebApp.Pages
{
    public class QuizStartModel : PageModel
    {
        private readonly IQuizService quizService;
        private readonly IQuizCategoryService categoryService;

        public QuizStartModel(IQuizService quizService, IQuizCategoryService categoryService)
        {
            this.quizService = quizService;
            this.categoryService = categoryService;
        }

        [BindProperty]
        public QuizStartViewModel Quiz { get; set; } = new();

        public List<SelectListItem> Categories { get; set; } = new();
        public List<SelectListItem> Difficulties { get; set; } = new();
        public List<SelectListItem> Types { get; set; } = new();

        public async Task OnGetAsync()
        {
            if (HttpContext.Session.GetString("UserId") == null)
            {
                HttpContext.Session.SetString("UserId", Guid.NewGuid().ToString());
            }
            await LoadDataAsync();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadDataAsync();
                return Page();
            }
            var category = Quiz.CategoryId.HasValue
                ? categoryService.GetById(Quiz.CategoryId.Value)
                : null;

            var quiz = await quizService.GetQuizAsync(
                (short)Quiz.Amount,
                category,
                Quiz.Difficulty,
                Quiz.Type
            );

            HttpContext.Session.SetString(
                "CurrentQuiz",
                JsonConvert.SerializeObject(quiz)
            );

            HttpContext.Session.SetInt32("CurrentQuestionIndex", 0);

            return RedirectToPage("QuizGame");
        }

        private async Task LoadDataAsync()
        {
            await categoryService.InitializeAsync();

            Categories = categoryService.GetAll()
                .Select(c => new SelectListItem(c.Name, c.Id.ToString()))
                .ToList();

            Difficulties = Enum.GetValues<QuizDifficulty>()
                .Select(d => new SelectListItem(d.ToString(), d.ToString()))
                .ToList();

            Types = Enum.GetValues<QuizType>()
                .Select(t => new SelectListItem(t.ToString(), t.ToString()))
                .ToList();
        }
    }
}