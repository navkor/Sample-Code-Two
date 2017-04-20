using BP.VM.ViewModels.Authentication;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BP.Web.Controllers
{
    public class HeaderController : Controller
    {
        LoggedInUserVM UserVM;
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;
        public HeaderController() { }
        public HeaderController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;
        }
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        // GET: Header
        [ChildActionOnly]
        [Route("Header/Navigation")]
        public PartialViewResult Navigation()
        {
            // first, get the userId
            var userId = User.Identity.GetUserId();
            var rolesList = RoleManager.Roles.OrderBy(y => y.Index).ToList();
            var roleName = "";
            var currentIndex = 0;
            var roleGroupIndex = 0;
            foreach (var role in rolesList)
            {
                if (UserManager.IsInRole(userId, role.Name))
                {
                    roleName = role.Name;
                    roleGroupIndex = role.RoleGroup.Index;
                    currentIndex = role.Index;
                }
            }
            UserVM = new LoggedInUserVM {
                UserID = userId,
                RoleIndex = currentIndex,
                UserRole = roleName,
                RoleGroupIndex = roleGroupIndex,
                HasPassword = UserManager.HasPassword(userId)
            };
            return PartialView("navigationPartial", UserVM);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }

                if (_roleManager != null)
                {
                    _roleManager.Dispose();
                    _roleManager = null;
                }
            }
            base.Dispose(disposing);
        }
    }
}