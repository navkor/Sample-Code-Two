using System.Web;
using System.Web.SessionState;

namespace BP
{
    public static class SessionExtensions
    {
        public static T GetDataFromSession<T>(this HttpSessionStateBase session, string key)
        {
            if (session.Count > 0)
                return (T)session[key];
            else
                return default(T);
        }

        public static void SetDataToSession<T>(this HttpSessionStateBase session, string key, object value)
        {
            if (session[key] != null) session[key] = value;
            else session.Add(key, value);
        }
    }
}
