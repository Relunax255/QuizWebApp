using QuizWebApp.Models;

namespace QuizWebApp.Services
{
    public interface IQuizService
    {
        public Task<Quiz> GetQuizAsync(short amount,
    Category? category = null,
    QuizDifficulty difficulty = QuizDifficulty.Unspecified,
    QuizType type = QuizType.Unspecified);
    }
    public class QuizService : IQuizService
    {
        IQuizApiClient apiClient;
        IQuizCategoryService categoryService;
        public QuizService (IQuizApiClient apiClient, IQuizCategoryService categoryService)
        {
            this.apiClient = apiClient;
            this.categoryService = categoryService;
        }
        public async Task<Quiz> GetQuizAsync(
            short amount,
            Category? category = null,
            QuizDifficulty difficulty = QuizDifficulty.Unspecified,
            QuizType type = QuizType.Unspecified)
        {
            var dto = await apiClient.GetQuizAsync(
                amount,
                category?.Id,
                difficulty != QuizDifficulty.Unspecified ? QuizDifficultyToString(difficulty) : null,
                type != QuizType.Unspecified ? QuizTypeToString(type) : null
            );

            var quiz = new Quiz
            {
                Questions = dto.results.Select(item => new Question
                {
                    Type = QuizTypeFromString(item.type),
                    Difficulty = QuizDifficultyFromString(item.difficulty),
                    Category = categoryService.GetByName(item.category),
                    QuestionText = item.question,

                    Answers = item.incorrect_answers
                        .Append(item.correct_answer)
                        .Select(a => new Answer
                        {
                            AnswerText = a,
                            IsCorrect = a == item.correct_answer
                        })
                        .OrderBy(_ => Guid.NewGuid()) // optional shuffle
                        .ToList()
                }).ToList()
            };

            return quiz;
        }
        #region categoriesandtypesstring
        private static string QuizDifficultyToString(QuizDifficulty difficulty)
        {
            switch (difficulty)
            {
                case QuizDifficulty.Easy: return "easy";
                case QuizDifficulty.Medium: return "medium";
                case QuizDifficulty.Hard: return "hard";
                default: throw new Exception("Quiz difficulty is impossible");
            }
        }

        private static string QuizTypeToString(QuizType type)
        {
            switch (type)
            {
                case QuizType.MultipleChoice: return "multiple";
                case QuizType.TrueFalse: return "boolean";
                default: throw new Exception("Quiz type is null");
            }
        }
        private static QuizDifficulty QuizDifficultyFromString(string difficulty)
        {
            switch (difficulty)
            {
                case "easy":
                    return QuizDifficulty.Easy;
                case "medium":
                    return QuizDifficulty.Medium;
                case "hard":
                    return QuizDifficulty.Hard;
                default:
                    throw new Exception("Invalid quiz difficulty string");
            }
        }

        private static QuizType QuizTypeFromString(string type)
        {
            switch (type)
            {
                case "multiple":
                    return QuizType.MultipleChoice;
                case "boolean":
                    return QuizType.TrueFalse;
                default:
                    throw new Exception("Invalid quiz type string");
            }
        }
        #endregion
    }
}
