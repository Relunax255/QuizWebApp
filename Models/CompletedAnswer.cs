namespace QuizWebApp.Models
{
    public class CompletedAnswer
    {
        public int Id { get; set; }

        public int CompletedQuestionId { get; set; }
        public CompletedQuestion CompletedQuestion { get; set; }

        public string AnswerText { get; set; }

        public bool IsCorrect { get; set; }

        public bool IsSelected { get; set; }
    }
}
