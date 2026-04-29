using Newtonsoft.Json;
using QuizWebApp.Models;
using System.Xml.Linq;

namespace QuizWebApp.Services
{
    public interface IQuizCategoryService
    {
        Task InitializeAsync();
        Category? GetById(int id);
        Category? GetByName(string name);
        IReadOnlyCollection<Category> GetAll();
    }
    public class QuizCategoryService : IQuizCategoryService
    {
        private IQuizApiClient apiClient;

        private Dictionary<int, Category> categories = new();

        public QuizCategoryService(IQuizApiClient apiClient)
        {
            this.apiClient = apiClient;
        }

        public async Task InitializeAsync()
        {
            if (categories.Count == 0)
            {
                var dtos = await apiClient.GetCategoriesAsync();
                if (dtos != null)
                {
                    categories = dtos.trivia_categories.ToDictionary(
                    c => c.id,
                    c => new Category
                    {
                        Id = c.id,
                        Name = c.name
                    });

                }
            }
        }

        public Category? GetById(int id)
        {
            return categories.Values.Where(c => c.Id == id).SingleOrDefault();
        }
        public Category? GetByName(string name)
        {
            return categories.Values.Where(c => c.Name == name).SingleOrDefault();
        }

        public IReadOnlyCollection<Category> GetAll()
        {
            return categories.Values;
        }
    }
}
