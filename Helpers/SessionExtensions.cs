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
}