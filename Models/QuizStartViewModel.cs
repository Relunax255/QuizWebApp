using System.ComponentModel.DataAnnotations;
namespace QuizWebApp.Models
{
    public class QuizStartViewModel
    {
        [Range(1, 50)]
        public int Amount { get; set; }

        public int? CategoryId { get; set; }

        [Required]
        public QuizDifficulty Difficulty { get; set; }

        [Required]
        public QuizType Type { get; set; }
    }
}
