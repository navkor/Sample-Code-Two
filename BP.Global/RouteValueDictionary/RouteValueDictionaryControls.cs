using System.Collections.Generic;
using System.Web.Routing;

namespace BP
{
    public class CRUDRoutingValues
    {
        public string RouteName { get; set; }
        public RouteValueDictionary RouteValueDictionary { get; set; }
        public IDictionary<string, object> HTMLValues { get; set; }
        public Dictionary<string, object> CreateButtons()
        {
            return new Dictionary<string, object>()
            { {
                    "class", "btn btn-primary"
            } };
        }

        public Dictionary<string, object> EditLink()
        {
            return new Dictionary<string, object>() { {
                    "class", "btn btn-success"
            } };
        }

        public Dictionary<string, object> DetailsLink()
        {
            return new Dictionary<string, object>() { {
                    "class", "btn btn-warning"
            } };
        }

        public Dictionary<string, object> DeleteLink()
        {
            return new Dictionary<string, object>() { {
                    "class", "btn btn-danger"
            } };
        }

        public Dictionary<string, object> BackLink()
        {
            return new Dictionary<string, object>() { {
                    "class", "btn btn-info"
            } };
        }
    }
}
