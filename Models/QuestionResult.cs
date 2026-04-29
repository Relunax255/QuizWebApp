namespace QuizWebApp.Models
{
    public class QuestionResult
    {
        public int QuestionId { get; set; }

        public int SelectedAnswerId { get; set; }

        public bool IsCorrect { get; set; }
    }
}