using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using QuizWebApp.Models;
using QuizWebApp.Services;
using QuizWebApp.Helpers;

namespace QuizWebApp.Pages
{
    public class QuizStartModel : PageModel
    {
        private readonly IQuizService quizService;
        private readonly IQuizCategoryService categoryService;
        private readonly QuizDbContext db;

        public QuizStartModel(IQuizService quizService, IQuizCategoryService categoryService, QuizDbContext db)
        {
            this.quizService = quizService;
            this.categoryService = categoryService;
            this.db = db;
        }

        [BindProperty]
        public QuizStartViewModel Quiz { get; set; } = new();

        public List<SelectListItem> Categories { get; set; } = new();
        public List<SelectListItem> Difficulties { get; set; } = new();
        public List<SelectListItem> Types { get; set; } = new();

        public async Task OnGetAsync()
        {
            await LoadDataAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadDataAsync();
                return Page();
            }

            var quizObj = await quizService.GetQuizAsync(
                (short)Quiz.Amount,
                Quiz.CategoryId.HasValue ? categoryService.GetById(Quiz.CategoryId.Value) : null,
                Quiz.Difficulty,
                Quiz.Type
            );

            var userId = HttpContext.GetOrCreateUserId();

            var session = await db.QuizSessions
                .FirstOrDefaultAsync(s => s.UserId == userId);

            if (session == null)
            {
                session = new QuizSession { UserId = userId };
                db.QuizSessions.Add(session);
            }

            session.QuizJson = JsonConvert.SerializeObject(quizObj);
            session.ResultsJson = JsonConvert.SerializeObject(new List<QuestionResult>());
            session.CurrentQuestionIndex = 0;
            await db.SaveChangesAsync();

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