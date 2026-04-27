using System.ComponentModel.DataAnnotations;
namespace QuizWebApp.Models
{
    public class Answer
    {
        [Key]
        public int AnswerId { get; set; }
        [Required]
        public int QuestionId { get; set; }
        [Required]
        public string AnswerText { get; set; }
        [Required]
        public bool IsCorrect { get; set; }
        public Question Question { get; set; }
    }
}
