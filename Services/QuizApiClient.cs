using Newtonsoft.Json;
using QuizWebApp.Models;
using System.Net.Http;
using System.Web;

namespace QuizWebApp.Services
{
    public interface IQuizApiClient
    {
        public Task<RootCategories?> GetCategoriesAsync();
        public Task<RootQuiz?> GetQuizAsync(short amount,
            int? categoryId = null,
            string? difficulty = null,
            string? type = null);
    }
    public class QuizApiClient : IQuizApiClient
    {
        static string quizzesUrl = "https://opentdb.com/api.php";
        static string categoriesUrl = "https://opentdb.com/api_category.php";
        public QuizApiClient(HttpClient hc)
        {
            httpClient = hc;
        }
        public async Task<RootCategories?> GetCategoriesAsync()
        {
            var content = await getContentUsingHttp(categoriesUrl);
            if (content == null) return null;
            return JsonConvert.DeserializeObject<RootCategories>(content);
        }
        public async Task<RootQuiz> GetQuizAsync(
            short amount,
            int? categoryId = null,
            string? difficulty = null,
            string? type = null)
        {
            var parameters = new Dictionary<string, string>
            {
                ["amount"] = amount.ToString()
            };

            if (categoryId.HasValue)
                parameters["category"] = categoryId.Value.ToString();

            if (!string.IsNullOrEmpty(difficulty))
                parameters["difficulty"] = difficulty;

            if (!string.IsNullOrEmpty(type))
                parameters["type"] = type;

            var response = await getContentUsingHttp(quizzesUrl, parameters);

            return JsonConvert.DeserializeObject<RootQuiz>(response);
        }
        #region http
        readonly HttpClient httpClient;

        static int maxAttempts = 3;

        private async Task<string> getContentUsingHttp(string url, Dictionary<string, string> parameters = null)
        {
            if (parameters != null)
            {
                if (parameters.Count > 0)
                {
                    url = createUrlWithParams(url, parameters);
                }
            }
            
            int attempts = 0;
            while (attempts < maxAttempts)
            {
                try
                {
                    using var request = new HttpRequestMessage(HttpMethod.Get, url);
                    using var response = await httpClient.SendAsync(request);

                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsStringAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception: {ex.Message}");
                    attempts++;
                }
            }
            return String.Empty;
        }
        static string createUrlWithParams(string baseUrl, Dictionary<string, string> parameters)
        {
            var uriBuilder = new UriBuilder(baseUrl);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);

            foreach (var param in parameters)
            {
                query[param.Key] = param.Value;
            }
            uriBuilder.Query = query.ToString();

            return uriBuilder.ToString();
        }
        #endregion http
    }
    public class RootQuiz
    {
        public int response_code { get; set; }
        public List<Result> results { get; set; }
    }
    public class Result
    {
        public string type { get; set; }
        public string difficulty { get; set; }
        public string category { get; set; }
        public string question { get; set; }
        public string correct_answer { get; set; }
        public List<string> incorrect_answers { get; set; }
    }
    public class RootCategories
    {
        public List<TriviaCategory> trivia_categories { get; set; }
    }

    public class TriviaCategory
    {
        public int id { get; set; }
        public string name { get; set; }
    }
}
