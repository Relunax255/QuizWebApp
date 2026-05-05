namespace QuizWebApp.Helpers
{
    public static class SessionExtensions
    {
        public static string GetOrCreateUserId(this ISession session)
        {
            var userId = session.GetString("UserId");

            if (string.IsNullOrEmpty(userId))
            {
                userId = Guid.NewGuid().ToString();
                session.SetString("UserId", userId);
            }

            return userId;
        }
    }
    public static class HttpContextExtensions
    {
        public static string GetOrCreateUserId(this HttpContext httpContext)
        {
            const string key = "UserId";

            if (!httpContext.Session.TryGetValue(key, out _))
            {
                var id = Guid.NewGuid().ToString();
                httpContext.Session.SetString(key, id);
            }

            return httpContext.Session.GetString(key);
        }
    }
}