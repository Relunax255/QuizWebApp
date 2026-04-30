using System.ComponentModel.DataAnnotations;
namespace QuizWebApp.Models
{
    public class Answer
    {
        static int currentId=0;
        [Key]
        public int AnswerId { get; set; }
        [Required]
        public int QuestionId { get; set; }
        [Required]
        public string AnswerText { get; set; }
        [Required]
        public bool IsCorrect { get; set; }
        public Question Question { get; set; }

        public Answer()
        {
            AnswerId = currentId++;
            currentId++;
        }
    }
}
