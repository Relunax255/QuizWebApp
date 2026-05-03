namespace QuizWebApp.Models
{
    public class CompletedQuiz
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string? UserId { get; set; } 

        public List<CompletedQuestion> Questions { get; set; } = new();
    }

}
