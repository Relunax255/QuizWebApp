using System.ComponentModel.DataAnnotations;

namespace QuizWebApp.Models
{
    public class Quiz
    {
        [Key]
        public int Id { get; set; }
        public List<Question> Questions { get; set; }
    }
}
