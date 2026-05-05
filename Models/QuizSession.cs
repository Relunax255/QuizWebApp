namespace QuizWebApp.Models
{
    public class QuizSession
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string QuizJson { get; set; } = string.Empty;
        public string ResultsJson { get; set; } = string.Empty;
        public int CurrentQuestionIndex { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
