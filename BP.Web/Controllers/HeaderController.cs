using BP.Service.Providers.Authentication;
using BP.VM.ViewModels.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BP.Web.Controllers
{
    public class HeaderController : Controller
    {
        UserProvider _user;
        LoggedInUserVM UserVM;
        public HeaderController()
        {
            _user = new UserProvider();
        }
        // GET: Header
        [ChildActionOnly]
        [Route("Header/Navigation")]
        public PartialViewResult Navigation()
        {
            UserVM = _user.LoggedInUser().Result;
            return PartialView("partials/navigation", UserVM);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_user != null)
                {
                    _user.Dispose();
                    _user = null;
                }
            }
            base.Dispose(disposing);
        }
    }
}