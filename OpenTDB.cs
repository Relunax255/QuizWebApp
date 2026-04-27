using QuizWebApp.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using Newtonsoft.Json;

namespace QuizWebApp
{
    public static class OpenTDB
    {
        static string quizzesUrl = "https://opentdb.com/api.php";
        static string categoriesUrl = "https://opentdb.com/api_category.php";

        public static async Task<IEnumerable<Category>> GetCategories()
        {
            var content = await getContentUsingHttp(categoriesUrl);
            if (content==null) return Enumerable.Empty<Category>();
            var rootCategories = JsonConvert.DeserializeObject<RootCategories>(content);
            List<Category> categories = new List<Category>();
            foreach (var category in rootCategories.trivia_categories)
            {
                categories.Add(new Category { Id = category.id, Name = category.name });
            }
            return categories;
        }
        #region http-requests
        static HttpClient httpClient = new HttpClient();
        
        static int maxAttempts = 3;
        
        private static async Task<string> getContentUsingHttp(string url, Dictionary<string, string> parameters = null)
        {
            if (parameters!=null)
            {
                if (parameters.Count>0)
                {
                    url = createUrlWithParams(url, parameters);
                }
            }
            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            using var response = await httpClient.SendAsync(request);
            int attempts = 0;
            while (attempts < maxAttempts) 
            {
                try
                {
                    response.EnsureSuccessStatusCode();
                    string result = await response.Content.ReadAsStringAsync();

                    return result;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception in getting content: {ex.Message}");
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
        #endregion
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
