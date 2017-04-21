using BP.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Linq;
using System.Web.Routing;

namespace System.Web.Mvc
{

    public class RoleAuthorizeAttribute : AuthorizeAttribute
    {
        public int RoleIndex { get; set; }
        public RoleAuthorizeAttribute()
        {

        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (AuthorizeRequest(filterContext)) return;
            filterContext.Result = new RedirectToRouteResult("Default", new RouteValueDictionary(new { controller = "Account", action = "UnAuthorized" }));
            filterContext.Controller.TempData["notallowed"] = "true";
            HandleUnauthorizedRequest(filterContext);
        }

        private bool AuthorizeRequest(AuthorizationContext filterContext)
        {
            using (var context = new ApplicationDbContext())
            {
                var userId = filterContext.RequestContext.HttpContext.User.Identity.GetUserId();
                var store = new RoleStore<ApplicationRole>(context);
                var manager = new RoleManager<ApplicationRole>(store);
                var roles = manager.Roles.OrderBy(x => x.Index).Where(y => y.Index >= RoleIndex).ToList();
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);
                foreach (var role in roles)
                {
                    if (userManager.IsInRole(userId, role.Name)) return true;
                }
                return false;
            }
        }
    }
    public class RoleGroupAuthorizeAttribute : AuthorizeAttribute
    {
        public int GroupIndex { get; set; }
        public RoleGroupAuthorizeAttribute() { }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                //if not logged, it will work as normal Authorize and redirect to the Login
                base.HandleUnauthorizedRequest(filterContext);

            }
            else
            {
                //logged and wihout the role to access it - redirect to the custom controller action
                filterContext.Controller.TempData["notallowed"] = "true";
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "UnAuthorized", area = "" }));
            }
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (AuthorizeRequest(filterContext)) return;
            filterContext.Result = new RedirectToRouteResult("Default", new RouteValueDictionary(new { controller = "Account", action = "UnAuthorized" }));
            filterContext.Controller.TempData["notallowed"] = "true";
            HandleUnauthorizedRequest(filterContext);
        }

        private bool AuthorizeRequest(AuthorizationContext filterContext)
        {
            using (var context = new ApplicationDbContext())
            {
                var userId = filterContext.RequestContext.HttpContext.User.Identity.GetUserId();
                var store = new RoleStore<ApplicationRole>(context);
                var manager = new RoleManager<ApplicationRole>(store);
                var roles = manager.Roles.OrderBy(x => x.Index).Where(y => y.RoleGroup.Index >= GroupIndex).ToList();
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);
                foreach (var role in roles)
                {
                    if (userManager.IsInRole(userId, role.Name)) return true;
                }
                return false;
            }
        }
    }
}