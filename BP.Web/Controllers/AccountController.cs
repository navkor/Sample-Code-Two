using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using BP.Web.Models;
using BP.Service.Providers.Logger;
using reCaptcha;
using System.Configuration;

namespace BP.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        CoreLoggerProvider _logger;
        private ApplicationRoleManager _roleManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ApplicationRoleManager roleManager, CoreLoggerProvider logger)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;
            _logger = logger;
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
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            LoginViewModel model = new LoginViewModel();
            model.pageTitle = "Login Step One";
            model.Instructions = "Enter your username or email below.<br />This is a two step process.";
            model.Warning = "";
            model.buttonValue = "Next";
            model.partialView = "partials/loginOnePartial";
            model.step = 1;
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.IPAddress = Server.HtmlEncode(Request.UserHostAddress);
            return View(model);
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl, int step)
        {
            var ipAddress = Server.HtmlEncode(Request.UserHostAddress);
            var subject = "Account Login";
            var instigator = "Login System";
            var system = "Account Controller";
            ViewBag.IPAddress = ipAddress;
            var lookupUserName = "";
            if (step == 1)
            {
                if (!string.IsNullOrEmpty(model.Email))
                {
                    lookupUserName = await GetUserUserName(model.Email);
                    if (UserManager.Users.Any(y => y.UserName == lookupUserName))
                    {
                        model.pageTitle = "Login Step Two";
                        model.Instructions = "Now enter your password below.";
                        model.Warning = "Be careful!  Too many wrong moves will lock out your account.";
                        model.partialView = "partials/loginTwoPartial";
                        model.buttonValue = "Log in";
                        model.step = 2;
                        return View(model);
                    }
                }
                ModelState.AddModelError(nameof(LoginViewModel.Email), "Please include a username before continuing.");
                return View(model);
            } else
            {
                    if (string.IsNullOrEmpty(model.Password)) ModelState.AddModelError(nameof(LoginViewModel.Password), "Please include a password before continuing.");
            }
            model.step = step;
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            lookupUserName = await GetUserUserName(model.Email);
            var result = await SignInManager.PasswordSignInAsync(lookupUserName, model.Password, model.RememberMe, shouldLockout: true);
            switch (result)
            {
                case SignInStatus.Success:
                    await Logger.CreateNewLog($"{model.Email} successfully logged in using Username/Password from {ipAddress}", subject, instigator, system);
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    await Logger.CreateNewLog($"{model.Email} account locked out using Username/Password from {ipAddress}", subject, instigator, system);
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    await Logger.CreateNewLog($"{model.Email} requires verification before logging in using Username/Password from {ipAddress}", subject, instigator, system);
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    await Logger.CreateNewLog($"{model.Email} failed log in using Username/Password from {ipAddress}", subject, instigator, system);
                    ViewBag.Message = "Please be careful!  Too many failed attemps will lock your account.";
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        internal async Task<string> GetUserUserName(string userName)
        {
            var returnValue = userName;
            if (userName.Contains("@"))
            {
                var user = await UserManager.FindByEmailAsync(userName);
                returnValue = user.UserName;
            }
            return returnValue;
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            ViewBag.IPAddress = Server.HtmlEncode(Request.UserHostAddress);
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            var IPAddress = Server.HtmlEncode(Request.UserHostAddress);
            ViewBag.IPAddress = IPAddress;
            var subject = "Registration Code Verification";
            var instigator = "Registration System";
            var system = "Account Controller";
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent:  model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            ViewBag.Recaptcha = ReCaptcha.GetHtml(ConfigurationManager.AppSettings["ReCaptcha:SiteKey"]);
            ViewBag.publicKey = ConfigurationManager.AppSettings["ReCaptcha:SiteKey"];
            ViewBag.IPAddress = Server.HtmlEncode(Request.UserHostAddress);
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            var IPAddress = Server.HtmlEncode(Request.UserHostAddress);
            ViewBag.IPAddress = IPAddress;
            var subject = "Account Registration";
            var instigator = "Registration System";
            var system = "Account Controller";
            if (ModelState.IsValid && ReCaptcha.Validate(ConfigurationManager.AppSettings["ReCaptcha:SecretKey"]))
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await Logger.CreateNewLog($"Successful registration for {model.Email} on IPAddress {IPAddress}", subject, instigator, system);
                   // await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);

                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + $"\">{callbackUrl}</a>.<br />You can also copy and paste it to your browser if your email does not allow you to click the link.");
                    ViewBag.RegisteredEmail = model.Email;
                    return View("RegistrationSuccessful");
                }
                AddErrors(result);
            }
            await Logger.CreateNewLog($"Failed registration for {model.Email} on IPAddress {IPAddress}", subject, instigator, system);
            ViewBag.RecaptchaLastErrors = ReCaptcha.GetLastErrors(this.HttpContext);

            ViewBag.publicKey = ConfigurationManager.AppSettings["ReCaptcha:SiteKey"];

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            var IPAddress = Server.HtmlEncode(Request.UserHostAddress);
            ViewBag.IPAddress = IPAddress;
            var subject = "Email Confirmed";
            var instigator = "Registration System";
            var system = "Account Controller";
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(userId);
                await Logger.CreateNewLog($"Successfully confirmed email for {user.UserName} on IPAddress {IPAddress}", subject, instigator, system);
                return View("ConfirmEmail");
            }
            await Logger.CreateNewLog($"Failed confirmed email for {userId} on IPAddress {IPAddress}", "Email Failed Confirmed", instigator, system);
            return View("Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            var IPAddress = Server.HtmlEncode(Request.UserHostAddress);
            ViewBag.IPAddress = IPAddress;
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            var IPAddress = Server.HtmlEncode(Request.UserHostAddress);
            ViewBag.IPAddress = IPAddress;
            var subject = "Forgot Email";
            var instigator = "Login System";
            var system = "Account Controller";
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + $"\">{callbackUrl}</a>.<br />You can also copy and paste the link to your URL if your email does not allow you to clink on links.");
                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            var IPAddress = Server.HtmlEncode(Request.UserHostAddress);
            ViewBag.IPAddress = IPAddress;
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            var IPAddress = Server.HtmlEncode(Request.UserHostAddress);
            ViewBag.IPAddress = IPAddress;
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            var IPAddress = Server.HtmlEncode(Request.UserHostAddress);
            ViewBag.IPAddress = IPAddress;
            var subject = "Reset Password";
            var instigator = "Login System";
            var system = "Account Controller";
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            var IPAddress = Server.HtmlEncode(Request.UserHostAddress);
            ViewBag.IPAddress = IPAddress;
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            var IPAddress = Server.HtmlEncode(Request.UserHostAddress);
            ViewBag.IPAddress = IPAddress;
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var IPAddress = Server.HtmlEncode(Request.UserHostAddress);
            ViewBag.IPAddress = IPAddress;
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            var IPAddress = Server.HtmlEncode(Request.UserHostAddress);
            ViewBag.IPAddress = IPAddress;
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var IPAddress = Server.HtmlEncode(Request.UserHostAddress);
            ViewBag.IPAddress = IPAddress;
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            var IPAddress = Server.HtmlEncode(Request.UserHostAddress);
            ViewBag.IPAddress = IPAddress;
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            var IPAddress = Server.HtmlEncode(Request.UserHostAddress);
            ViewBag.IPAddress = IPAddress;
            return View();
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

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}