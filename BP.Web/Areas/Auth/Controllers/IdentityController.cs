using BP.Service.Providers.Authentication;
using BP.Service.Providers.Core;
using BP.VM.PullModels.Authentication;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BP.Web.Areas.Auth.Controllers
{
    public class IdentityController : Controller
    {
        #region MyAccount Controller
        RegistrationProvider _provider;
        LoginProvider _login;
        UserProvider _user;
        SMTPProvider _smtp;
        public IdentityController()
        {
            _provider = new RegistrationProvider();
            _smtp = new SMTPProvider();
            _user = new UserProvider();
            _login = new LoginProvider();
        }

        [Route("Auth/Identity/ResetPassword")]
        public ActionResult ResetPassword()
        {
            var pullModel = new ResetPullModel();
            ViewBag.IPAddress = Server.HtmlEncode(Request.UserHostAddress);
            return View(pullModel);
        }

        [Route("Auth/Identity/ResetPassword")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPullModel pullModel)
        {
            if (ModelState.IsValid)
            {

            }
            ViewBag.IPAddress = Server.HtmlEncode(Request.UserHostAddress);
            pullModel.EmailAddress = "";
            return View(pullModel);
        }

        [Route("Auth/Identity/Register")]
        public ActionResult Register()
        {
            var pullModel = new RegisterAccount();
            ViewBag.IPAddress = Server.HtmlEncode(Request.UserHostAddress);
            return View(pullModel);
        }

        [Route("Auth/Identity/ConfirmEmail")]
        public async Task<ActionResult> ConfirmEmail(int? id, string code)
        {
            if (id == null)
            {
                return View("Error");
            }
            var methodResults = await _provider.ActivateAccount(new ActivateEmailModel
            {
                ID = id ?? 0,
                ActivateString = code
            }, Server.HtmlEncode(Request.UserHostAddress)).ConfigureAwait(continueOnCapturedContext: false);
            if (methodResults.Success) return View("ActivateSuccessful");
            return View("ActivationFailed");
        }

        [Route("Auth/Identity/Register")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterAccount pullModel)
        {
            if (ModelState.IsValid)
            {
                // everything is good, so let's continue
                var methodResults = await _provider.RegisteringAccount(pullModel).ConfigureAwait(continueOnCapturedContext: false);
                if (methodResults.Success)
                {
                    // let's get the message sent to their email with the activation code
                    var callbackUrl = Url.Action("ConfirmEmail", "Identity", new { id = methodResults.ID, code = methodResults.Message }, protocol: Request.Url.Scheme);
                    await _smtp.SendNewEmail("noreply@basicallyprepared.com", pullModel.Email, "Basically Prepared", "", "Confirm you account", $"Please confirm you account by clicking the following link, or copying and pasting it into your browser's address bar <a href=\"{callbackUrl}\">{callbackUrl}</a>", "new account registration");
                    ViewBag.EmailAddress = pullModel.Email;
                    return View("AccountCreated");
                }
                else
                {
                    ModelState.AddModelError("", methodResults.Message);
                }
            }
            pullModel.Password = "";
            pullModel.ConfirmPassword = "";
            pullModel.ConfirmEmail = "";
            ViewBag.IPAddress = Server.HtmlEncode(Request.UserHostAddress);
            return View(pullModel);
        }

        [Route("Auth/Identity/Login")]
        public ActionResult Login()
        {
            var pullModel = new LoginAccountStep1();
            ViewBag.IPAddress = Server.HtmlEncode(Request.UserHostAddress);
            return View("Login", pullModel);
        }

        [Route("Auth/Identity/Login")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginAccountStep1 pullModel)
        {
            ViewBag.IPAddress = Server.HtmlEncode(Request.UserHostAddress);
            if (ModelState.IsValid)
            {
                // everything is good, so let's get crackin'
                var registration = await _login.LoginAccountStepOne(pullModel).ConfigureAwait(continueOnCapturedContext: false);
                if (registration.ID > 0)
                {
                    TempData["salt"] = registration.IdentityAttribute.Salt;
                    var login2 = new LoginAccountStep2
                    {
                        Email = pullModel.Email
                    };
                    return View("Login2", login2);
                }
                ModelState.AddModelError("", "Login was unsuccessful.  Please try again carefully.  Too many failed attempts will lock your account.");
            }
            pullModel.Email = "";
            return View(pullModel);
        }

        [Route("Account/Login2")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login2(LoginAccountStep2 pullModel)
        {
            var salt = TempData["salt"].ToString();
            ViewBag.IPAddress = Server.HtmlEncode(Request.UserHostAddress);
            if (ModelState.IsValid)
            {
                var loggedIn = await _login.LoginAccountStepTwo(pullModel, salt, HttpContext.Session, Server.HtmlEncode(Request.UserHostAddress)).ConfigureAwait(continueOnCapturedContext: false);
                if (loggedIn.Registration.ID > 0)
                {
                    if (pullModel.RememberMe)
                    {
                        // TODO  need to figure out why the cookie isn't being saved to the browser!
                        if (loggedIn.Cookie.Name != "null")
                        {
                            Response.SetCookie(loggedIn.Cookie);
                        }
                    }
                    return RedirectToAction("Index", "Home", new { area = "" });
                    
                }
                ModelState.AddModelError("", "Login was unsuccessful.  Please try again carefully.  Too many failed attempts will lock your account.");
            }
            pullModel.Password = "";
            TempData["salt"] = salt;
            return View(pullModel);
        }

        [Route("Auth/Identity/LogOff")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LogOff()
        {
            // first, let's go in a clear out the session value
            var cookie = await _user.LogUserOff(HttpContext.Session).ConfigureAwait(continueOnCapturedContext: false);
            if (!cookie.Name.Contains("none"))
            {
                Response.SetCookie(cookie);
            }
            return RedirectToAction("Index", "Home", new { area = "" });
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_provider != null)
                {
                    _provider.Dispose();
                    _provider = null;
                }
                if (_login != null)
                {
                    _login.Dispose();
                    _login = null;
                }
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