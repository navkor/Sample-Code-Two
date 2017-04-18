using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BP.Web.Areas.Users.Controllers
{
    [Authorize]
    public class RegisteredController : Controller
    {
        // GET: Users/Registered
        public ActionResult Index()
        {
            return View();
        }

        [Route("users/registered/profile")]
        // GET: Users/Registered/Profile
        public new ActionResult Profile()
        {
            return View();
        }
    }
}