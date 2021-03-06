﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using sampleLogin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace sampleLogin.Extensions
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
                foreach(var role in roles)
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
        public RoleGroupAuthorizeAttribute()
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