using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BP.Web.Areas.Auth.Controllers
{
    public class UserController : Controller
    {
        // GET: Auth/User
        public ActionResult Login()
        {
            return View();
        }

        [Authorize]
        public ActionResult Logout()
        {
            HttpContext.GetOwinContext().Authentication.SignOut();
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        [Authorize]
        public ActionResult Claims()
        {
            return View();
        }
    }
}