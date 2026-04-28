using System.ComponentModel.DataAnnotations;

namespace QuizWebApp.Models
{
    public class Question
    {
        [Key]
        public int QuestionId { get; set; }
        [Required]
        public QuizType Type { get; set; }
        [Required]
        public QuizDifficulty Difficulty { get; set; }
        [Required]
        public Category Category { get; set; }
        [Required]
        public string QuestionText { get; set; }

        // Navigation Property to related answers
        public List<Answer> Answers { get; set; }
    }
}
