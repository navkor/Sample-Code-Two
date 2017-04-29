using BP.Service.Providers.Core;
using BP.Service.Providers.Logger;
using BP.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BP.Web.Areas.Admin.Controllers
{
    [RoleGroupAuthorize(GroupIndex = 700)]
    public class UsersAdminController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        CoreLoggerProvider _logger;
        private ApplicationRoleManager _roleManager;
        private SMTPProvider _smtp;
        public UsersAdminController() { }
        public UsersAdminController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ApplicationRoleManager roleManager, CoreLoggerProvider logger, SMTPProvider smtp)
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

        [Route("Admin/UsersAdmin/ViewUsers")]
        public async Task<ActionResult> ViewUsers()
        {
            // this will return a list of users in the system and return them to the view
            var IPAddress = Server.HtmlEncode(Request.UserHostAddress);
            ViewBag.IPAddress = IPAddress;
            var myId = User.Identity.GetUserId();
            var users = await UserManager.Users.Where(x => x.Id != myId).Select(x => new UserModels {
                UserId = x.Id,
                UserName = x.UserName,
                EmailAddress = x.Email,
                EmailVerified = x.EmailConfirmed
            }).ToListAsync();
            return View(users);
        }

        [Route("Admin/UsersAdmin/CreateUsers")]
        public async Task<ActionResult> CreateUsers()
        {

            var IPAddress = Server.HtmlEncode(Request.UserHostAddress);
            ViewBag.IPAddress = IPAddress;
            var userId = User.Identity.GetUserId();
            var rolesList = await RoleManager.Roles.OrderBy(y => y.Index).ToListAsync();
            var model = new CreateUser
            {
                Roles = await RolesList(rolesList, userId)
            };
            return View(model);
        }

        [HttpPost]
        [Route("Admin/UsersAdmin/CreateUsers")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateUsers(CreateUser pullModel)
        {
            // they are creating a new user
            // so we gotta do things a little different for this one!
            var IPAddress = Server.HtmlEncode(Request.UserHostAddress);
            ViewBag.IPAddress = IPAddress;
            var subject = "Account Registration";
            var instigator = "User Admin Manager";
            var system = "UserAdmin Controller";
            if (ModelState.IsValid)
            {
                // they are ready to go!
                var user = new ApplicationUser
                {
                    UserName = pullModel.UserName,
                    Email = pullModel.EmailAddress
                };
                var result = await UserManager.CreateAsync(user, pullModel.PassWord);
                if (result.Succeeded)
                {
                    await UserManager.AddToRoleAsync(user.Id, pullModel.SelectedRole);
                    var adminUser = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    instigator = adminUser.Email;
                    await Logger.CreateNewLog($"Successfully created user: {pullModel.UserName} from IPAddress {IPAddress}.", subject, instigator, system);
                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    await SMTP.SendNewEmail("noreply@basicallyprepared.com", user.Email, "Basically Prepared", "", "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + $"\">{callbackUrl}</a>.<br />You can also copy and paste it to your browser if your email does not allow you to click the link.", "CreateAccount");
                    return RedirectToAction("ViewUsers", new { controller = "UsersAdmin", area = "Admin" });
                }
                AddErrors(result);
            }
            var userId = User.Identity.GetUserId();
            var rolesList = RoleManager.Roles.OrderBy(y => y.Index).ToList();
            pullModel.Roles = await RolesList(rolesList, userId);
            ViewBag.IPAddress = IPAddress;
            return View(pullModel);
        }

        [Route("Admin/UsersAdmin/EditUser")]
        public async Task<ActionResult> EditUser(string id)
        {

            var IPAddress = Server.HtmlEncode(Request.UserHostAddress);
            ViewBag.IPAddress = IPAddress;
            if (string.IsNullOrEmpty(id)) return View("Error");
            var user = await UserManager.FindByIdAsync(id);
            if (user == null) return View("UserNotFound");
            var roleName = "";
            var rolesList = await RoleManager.Roles.OrderBy(y => y.Index).ToListAsync();
            foreach (var role in rolesList)
            {
                if (UserManager.IsInRole(id, role.Name)) roleName = role.Name;
            }
            var model = new EditUser
            {
                UserId = user.Id,
                UserName = user.UserName,
                EmailAddress = user.Email,
                SelectedRole = roleName,
                Roles = await RolesList(rolesList, id)
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Admin/UsersAdmin/EditUser")]
        public async Task<ActionResult> EditUser(EditUser model)
        {
            var IPAddress = Server.HtmlEncode(Request.UserHostAddress);
            ViewBag.IPAddress = IPAddress;
            var subject = "User Edit";
            var instigator = "User Admin System";
            var system = "Admin Controller";
            if (ModelState.IsValid)
            {
                var current = await UserManager.FindByIdAsync(model.UserId);
                var emailToVerify = false; var keepRole = true;
                if (model.UserName != current.UserName) current.UserName = model.UserName;
                if (model.EmailAddress != current.Email)
                {
                    // they are changing the email address...so it needs to be verified before they can log in
                    emailToVerify = true;
                    current.Email = model.EmailAddress;
                    current.EmailConfirmed = false;
                }
                keepRole = await UserManager.IsInRoleAsync(current.Id, model.SelectedRole);
                var adminUser = new ApplicationUser();
                var result = await UserManager.UpdateAsync(current);
                if (result.Succeeded)
                {
                    bool success = true;
                    if (emailToVerify)
                    {
                        string code = await UserManager.GenerateEmailConfirmationTokenAsync(current.Id);
                        var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = current.Id, code = code }, protocol: Request.Url.Scheme);
                        await SMTP.SendNewEmail("noreply@basicallyprepared.com", current.Email, "Basically Prepared", "", "Confirm your new email", "Please confirm your new email address by clicking <a href=\"" + callbackUrl + $"\">{callbackUrl}</a>.<br />You can also copy and paste it to your browser if your email does not allow you to click the link.<br /><p>Until you confirm your email address, you won't be able to log into your account.</p>", "NewAccount");
                    }
                    if (!keepRole)
                    {
                        success = false;
                        var rolesQuickList = await RoleManager.Roles.OrderBy(x => x.Index).ToListAsync();
                        foreach(var role in rolesQuickList)
                        {
                            if (await UserManager.IsInRoleAsync(current.Id, role.Name)) await UserManager.RemoveFromRoleAsync(current.Id, role.Name);
                        }
                        var roleResult = await UserManager.AddToRoleAsync(current.Id, model.SelectedRole);
                        success = roleResult.Succeeded;
                        if (!success)
                        {
                            ModelState.AddModelError("", "Could not add user to selected role.");
                            await Logger.CreateNewLog($"Could not add {current.UserName} to role {model.SelectedRole} on {IPAddress}", subject, instigator, system);
                        }
                    }
                    if (success)
                    {
                        adminUser = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                        instigator = adminUser.Email;
                        await Logger.CreateNewLog($"Successfully updated and edited user {current.UserName} on {IPAddress}", subject, instigator, system);
                        return RedirectToAction("ViewUsers", new { controller = "UsersAdmin", area = "Admin" });
                    }
                }
                var sb = new StringBuilder();
                sb.Append("There were some errors with updating this user:");
                sb.AppendLine("");
                adminUser = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                instigator = adminUser.Email;
                await Logger.CreateNewLog($"Failed to update and edit user {current.UserName} on {IPAddress}", subject, instigator, system);
                foreach (var er in result.Errors)
                {
                    sb.AppendLine($"> {er}");
                }
                ModelState.AddModelError("", sb.ToString());
            }
            var rolesList = await RoleManager.Roles.OrderBy(y => y.Index).ToListAsync();
            model.Roles = await RolesList(rolesList, model.UserId);
            return View(model);
        }

        private async Task<IEnumerable<SelectListItem>> RolesList(IEnumerable<ApplicationRole> rolesList, string userId)
        {
            var currentIndex = 0;
            var maxIndex = 0;
            foreach (var role in rolesList)
            {
                if (await UserManager.IsInRoleAsync(userId, role.Name)) currentIndex = role.Index;
                if (await UserManager.IsInRoleAsync(User.Identity.GetUserId(), role.Name)) maxIndex = role.Index;
            }
            var returnList = rolesList.Where(x => x.Index <= maxIndex).OrderBy(y => y.Index).Select(n => new SelectListItem
            {
                Text = n.Name,
                Value = n.Name,
                Selected = n.Index == currentIndex
            });
            return returnList;
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


        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}