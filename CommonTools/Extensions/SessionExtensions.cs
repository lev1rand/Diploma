using Microsoft.AspNetCore.Http;

namespace CommonTools.Extensions
{
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.Set(key, value);
        }

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.Get<T>(key);
            return value;
        }
    }
}
