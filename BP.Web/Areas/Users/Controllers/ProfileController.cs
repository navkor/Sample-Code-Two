using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BP.Web.Areas.Users.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Users/Profile
        public ActionResult Index()
        {
            return View();
        }
    }
}