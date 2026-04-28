using QuizWebApp.Services;

namespace QuizWebApp.Pages.Quiz
{
    public class MainPageModel
    {
        IQuizService quizService;
        IQuizCategoryService quizCategoryService;
        public MainPageModel(IQuizService qs, IQuizCategoryService qcs) 
        { 
            this.quizService = qs; 
            this.quizCategoryService = qcs; 
        }

    }
}
