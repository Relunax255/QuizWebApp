namespace QuizWebApp.Models
{
    public class CompletedQuestion
    {
        public int Id { get; set; }

        public int CompletedQuizId { get; set; }
        public CompletedQuiz CompletedQuiz { get; set; }

        public string QuestionText { get; set; }

        public List<CompletedAnswer> Answers { get; set; } = new();

        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
