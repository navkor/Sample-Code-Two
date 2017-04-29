using BP.Service.Providers.Core;
using BP.Service.Providers.Logger;
using BP.Services.Providers.Account;
using BP.VM.ViewModels.AccountAdmin;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BP.Web.Areas.Admin.Controllers
{
    [RoleGroupAuthorize(GroupIndex = 700)]
    public class AccountAdminController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        CoreLoggerProvider _logger;
        private ApplicationRoleManager _roleManager;
        private SMTPProvider _smtp;

        public AccountAdminController()
        {

        }

        public AccountAdminController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ApplicationRoleManager roleManager, CoreLoggerProvider logger, SMTPProvider smtp)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;
            Logger = logger;
        }

        public SMTPProvider SMTP
        {
            get
            {
                return _smtp ?? new SMTPProvider();
            }
            private set
            {
                _smtp = value;
            }
        }


        public CoreLoggerProvider Logger
        {
            get
            {
                return _logger ?? new CoreLoggerProvider();
            }
            private set
            {
                _logger = value;
            }
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

        [Route("Admin/AccountAdmin/ViewAccounts")]
        public async Task<ActionResult> ViewAccounts()
        {
            // this will return a list of all accounts in the system
            var IPAddress = Server.HtmlEncode(Request.UserHostAddress);
            ViewBag.IPAddress = IPAddress;

            IEnumerable<AccountAdminViewModel> viewModel = new List<AccountAdminViewModel>();

            using (var provider = new AccountProvider(UserManager))
            {
                viewModel = await provider.ReturnFirstNAccounts(0);
            }

            return View(viewModel);
        }
    }
}