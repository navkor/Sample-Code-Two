﻿using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using BP.Web.Models;
using System.Collections.Generic;
using BP.Service.Providers.Logger;
using BP.Service.Providers.Core;
using System.Text;

namespace BP.Web.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        CoreLoggerProvider _logger;
        private ApplicationRoleManager _roleManager;
        private SMTPProvider _smtp;

        public ManageController()
        {
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ApplicationRoleManager roleManager, CoreLoggerProvider logger, SMTPProvider smtp)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;
            Logger = logger;
            SMTP = smtp;
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

        //
        // GET: /Manage/Index
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            var IPAddress = Server.HtmlEncode(Request.UserHostAddress);
            ViewBag.IPAddress = IPAddress;
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
                : message == ManageMessageId.Error ? "An error has occurred."
                : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
                : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                : "";

            var userId = User.Identity.GetUserId();
            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
                Logins = await UserManager.GetLoginsAsync(userId),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
            };
            return View(model);
        }

        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var IPAddress = Server.HtmlEncode(Request.UserHostAddress);
            ViewBag.IPAddress = IPAddress;
            var subject = "Link External Login";
            var system = "Manage Controller";
            var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
                var instigator = loginProvider;
                await Logger.CreateNewLog($"{User.Identity.Name} successfully removed an external login using {instigator} from {IPAddress}", subject, instigator, system);
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("ManageLogins", new { Message = message });
        }
        

        //
        // GET: /Manage/ChangeUserName
        [Route("Manage/ChangeUserName")]
        public ActionResult ChangeUserName()
        {
            var IPAddress = Server.HtmlEncode(Request.UserHostAddress);
            ViewBag.IPAddress = IPAddress;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Manage/ChangeUserName")]
        public async Task<ActionResult> ChangeUserName(EditUserName model)
        {
            var IPAddress = Server.HtmlEncode(Request.UserHostAddress);
            ViewBag.IPAddress = IPAddress;
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // let's find out if they've picked a username that already exists
            var check = await UserManager.FindByNameAsync(model.UserName);
            if (check != null)
            {
                ModelState.AddModelError(nameof(EditUserName.UserName), "That username is not available");
                return View(model);
            }
            // now, find out if the password is correct
            var current = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (await UserManager.CheckPasswordAsync(current, model.CurrentPassword))
            {
                current.UserName = model.UserName;
                var result = await UserManager.UpdateAsync(current);
                if (result.Succeeded) return RedirectToAction("Index");
                StringBuilder sb = new StringBuilder();
                sb.Append("There were some problems:");
                sb.AppendLine();
                foreach(var e in result.Errors)
                {
                    sb.AppendLine(e);
                }
                ModelState.AddModelError("", sb.ToString());
                return View(model);
            }
            ModelState.AddModelError(nameof(EditUserName.CurrentPassword), "Please try again.");
            return View(model);
        }

        private IEnumerable<SelectListItem> RolesList(IEnumerable<ApplicationRole> rolesList, string userId)
        {
            var currentIndex = 0;
            foreach (var role in rolesList)
            {
                if (UserManager.IsInRole(userId, role.Name)) currentIndex = role.Index;
            }
            var returnList = rolesList.Where(x => x.Index <= currentIndex).OrderBy(y => y.Index).Select(n => new SelectListItem
            {
                Text = n.Name,
                Value = n.Name
            });
            return returnList;
        }
        
        //
        // GET: /Manage/AddPhoneNumber
        public ActionResult AddPhoneNumber()
        {
            var IPAddress = Server.HtmlEncode(Request.UserHostAddress);
            ViewBag.IPAddress = IPAddress;
            return View();
        }

        //
        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            var IPAddress = Server.HtmlEncode(Request.UserHostAddress);
            ViewBag.IPAddress = IPAddress;
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Generate the token and send it
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), model.Number);
            if (UserManager.SmsService != null)
            {
                var message = new IdentityMessage
                {
                    Destination = model.Number,
                    Body = "Your security code is: " + code
                };
                await UserManager.SmsService.SendAsync(message);
            }
            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        }

        //
        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            var IPAddress = Server.HtmlEncode(Request.UserHostAddress);
            ViewBag.IPAddress = IPAddress;
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            var IPAddress = Server.HtmlEncode(Request.UserHostAddress);
            ViewBag.IPAddress = IPAddress;
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // GET: /Manage/VerifyPhoneNumber
        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var IPAddress = Server.HtmlEncode(Request.UserHostAddress);
            ViewBag.IPAddress = IPAddress;
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), phoneNumber);
            // Send an SMS through the SMS provider to verify the phone number
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            var IPAddress = Server.HtmlEncode(Request.UserHostAddress);
            ViewBag.IPAddress = IPAddress;
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId(), model.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
            }
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Failed to verify phone");
            return View(model);
        }

        //
        // POST: /Manage/RemovePhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemovePhoneNumber()
        {
            var IPAddress = Server.HtmlEncode(Request.UserHostAddress);
            ViewBag.IPAddress = IPAddress;
            var result = await UserManager.SetPhoneNumberAsync(User.Identity.GetUserId(), null);
            if (!result.Succeeded)
            {
                return RedirectToAction("Index", new { Message = ManageMessageId.Error });
            }
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            var IPAddress = Server.HtmlEncode(Request.UserHostAddress);
            ViewBag.IPAddress = IPAddress;
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            var IPAddress = Server.HtmlEncode(Request.UserHostAddress);
            ViewBag.IPAddress = IPAddress;
            var subject = "Account Password";
            var system = "Manage Controller";
            var instigator = "Change Account Password";
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (result.Succeeded)
            {
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                await Logger.CreateNewLog($"{user.UserName} successfully changed the password for his or her account on {IPAddress}", subject, instigator, system);
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            await Logger.CreateNewLog($"{user.UserName} failed to change the password for his or her account on {IPAddress}", subject, instigator, system);
            AddErrors(result);
            return View(model);
        }

        //
        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            var IPAddress = Server.HtmlEncode(Request.UserHostAddress);
            ViewBag.IPAddress = IPAddress;
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            var IPAddress = Server.HtmlEncode(Request.UserHostAddress);
            ViewBag.IPAddress = IPAddress;
            var subject = "Account Password";
            var system = "Manage Controller";
            var instigator = "Set Account Password";
            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (result.Succeeded)
                {
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    await Logger.CreateNewLog($"{user.UserName} successfully set a password for his or her account on {IPAddress}", subject, instigator, system);
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                await Logger.CreateNewLog($"{user.UserName} failed to set a password for his or her account on {IPAddress}", subject, instigator, system);
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Manage/ManageLogins
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            var IPAddress = Server.HtmlEncode(Request.UserHostAddress);
            ViewBag.IPAddress = IPAddress;
            ViewBag.StatusMessage =
                message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
        }

        //
        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var IPAddress = Server.HtmlEncode(Request.UserHostAddress);
            ViewBag.IPAddress = IPAddress;
            var subject = "Link External Login";
            var system = "Manage Controller";
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }
            var instigator = loginInfo.Login.LoginProvider;
            await Logger.CreateNewLog($"{User.Identity.Name} successfully linked an external login using {instigator} from {IPAddress}", subject, instigator, system);
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
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
            }

            base.Dispose(disposing);
        }

#region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

#endregion
    }
}