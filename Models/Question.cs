using System.ComponentModel.DataAnnotations;

namespace QuizWebApp.Models
{
    public class Question
    {
        [Key]
        public int QuestionID { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public string Difficulty { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public string QuestionText { get; set; }

        // Navigation Property to related answers
        public List<Answer> Answers { get; set; }
    }
}
