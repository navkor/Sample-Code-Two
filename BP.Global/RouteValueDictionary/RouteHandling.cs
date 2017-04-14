using System.Web.Routing;

namespace BP
{
    public class RouteHandling
    {
        public RouteValueDictionary GetRoutingValues(int id, string actionName, string controllerName)
        {
            return new RouteValueDictionary();
        }

        public RouteValueDictionary GetRoutingValues(int id, string actionName, string controllerName, int customerId)
        {
            return new RouteValueDictionary();
        }
    }
}
