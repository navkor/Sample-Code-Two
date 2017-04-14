using System.Web.SessionState;

namespace BP
{
    public static class SessionExtensions
    {
        public static T GetDataFromSession<T>(this HttpSessionState session, string key)
        {
            if (session.Count > 0)
                return (T)session[key];
            else
                return default(T);
        }

        public static void SetDataToSession<T>(this HttpSessionState session, string key, object value)
        {
            session[key] = value;
        }
    }
}
