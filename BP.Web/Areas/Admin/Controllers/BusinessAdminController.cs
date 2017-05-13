using BP.Service.Providers.Core;
using BP.Service.Providers.Logger;
using BP.Services.Providers.Business;
using BP.VM.ViewModels.Business;
using Microsoft.AspNet.Identity;
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
    public class BusinessAdminController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private BusinessProvider _provider;
        CoreLoggerProvider _logger;
        private ApplicationRoleManager _roleManager;
        private SMTPProvider _smtp;
        public BusinessAdminController() { }
        public BusinessAdminController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ApplicationRoleManager roleManager, CoreLoggerProvider logger, SMTPProvider smtp, BusinessProvider provider)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;
            Logger = logger;
            Provider = provider;
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

        public BusinessProvider Provider
        {
            get {
                return _provider ?? new BusinessProvider();
            }
            private set {
                _provider = value;
            }
        }

        [Route("Admin/BusinessAdmin/ViewBusiness")]
        public async Task<ActionResult> ViewBusiness()
        {
            var IPAddress = Server.HtmlEncode(Request.UserHostAddress);
            ViewBag.IPAddress = IPAddress;
            var businessList = await Provider.ViewAllBusinesses(UserManager.Users.Select(x => new NameStringId {
                ID = x.Id,
                Name = x.UserName
            }).ToList());
            return View(businessList);
        }

        [Route("Admin/BusinessAdmin/CreateBusiness")]
        public async Task<ActionResult> CreateBusiness()
        {
            var IPAddress = Server.HtmlEncode(Request.UserHostAddress);
            ViewBag.IPAddress = IPAddress;
            var pullModel = await Provider.UpdatePullModel(new BusinessPullModel());
            return View(pullModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Admin/BusinessAdmin/CreateBusiness")]
        public async Task<ActionResult> CreateBusiness(BusinessPullModel pullModel)
        {
            var IPAddress = Server.HtmlEncode(Request.UserHostAddress);
            ViewBag.IPAddress = IPAddress;
            var subject = "Create A Business";
            var instigator = "Admin Business Form";
            var system = "Business Admin Controller";
            if (ModelState.IsValid)
            {
                if (pullModel.BusinessTypeId > 0)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    instigator = user.Email;
                    // everything is just fine
                    var methodResults = await Provider.CreateNewBusiness(pullModel, IPAddress, instigator);
                    if (methodResults.Success)
                    {
                        return RedirectToAction("ViewBusiness");
                    }
                    ModelState.AddModelError("", methodResults.Message);
                }
                else ModelState.AddModelError(nameof(BusinessPullModel.BusinessTypeId), "Please select a business type before continuing...");
            }
            pullModel = await Provider.UpdatePullModel(pullModel);
            return View(pullModel);
        }

        [Route("Admin/BusinessAdmin/DeleteBusiness")]
        public async Task<ActionResult> DeleteBusiness(int? id)
        {
            var IPAddress = Server.HtmlEncode(Request.UserHostAddress);
            ViewBag.IPAddress = IPAddress;
            var viewModel = await Provider.BusinessById(id ?? 0);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Admin/BusinessAdmin/DeleteThisBusiness")]
        public async Task<ActionResult> DeleteThisBusiness(int id)
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            var instigator = user.Email;
            var IPAddress = Server.HtmlEncode(Request.UserHostAddress);
            var methodResults = await Provider.RemoveBusinessAccount(id, IPAddress, instigator);
            if (methodResults.Success) return RedirectToAction("ViewBusiness");
            return RedirectToRoute(new { controller = "BusinessAdmin", action = "DeleteBusiness", id = id, area = "Admin" });
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
                if (_logger != null)
                {
                    _logger.Dispose();
                    _logger = null;
                }
                if (_smtp != null)
                {
                    _smtp.Dispose();
                    _smtp = null;
                }
                if (_provider != null)
                {
                    _provider.Dispose();
                    _provider = null;
                }
            }

            base.Dispose(disposing);
        }


        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}