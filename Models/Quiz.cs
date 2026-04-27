using System.ComponentModel.DataAnnotations;

namespace QuizWebApp.Models
{
    public class Quiz
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public List<Question> Questions { get; set; }
    }
}
