using Newtonsoft.Json;
using QuizWebApp.Models;

namespace QuizWebApp.Services
{
    public interface IQuizCategoryService
    {
        Task InitializeAsync();
        Category? GetById(int id);
        IReadOnlyCollection<Category> GetAll();
    }
    public class QuizCategoryService : IQuizCategoryService
    {
        private readonly IQuizApiClient apiClient;

        private Dictionary<int, Category> categories = new();

        public QuizCategoryService(IQuizApiClient apiClient)
        {
            this.apiClient = apiClient;
        }

        public async Task InitializeAsync()
        {
            var dtos = await apiClient.GetCategoriesAsync();
            if (dtos != null)
            {
                List<Category> categories = new List<Category>();
                foreach (var category in dtos.trivia_categories)
                {
                    categories.Add(new Category { Id = category.id, Name = category.name });
                }
            }
        }

        public Category? GetById(int id)
        {
            return categories.TryGetValue(id, out var category)
                ? category
                : null;
        }

        public IReadOnlyCollection<Category> GetAll()
        {
            return categories.Values;
        }
    }
}
