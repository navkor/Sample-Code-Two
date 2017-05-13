using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using System.Threading.Tasks;

namespace BP.Web.Areas.Admin.Controllers
{
    [System.Web.Mvc.RoleGroupAuthorize(GroupIndex = 700)]
    public class InternalAPIController : ApiController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;
        private JsonHandlers _json;

        public InternalAPIController() { }

        public InternalAPIController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ApplicationRoleManager roleManager, JsonHandlers json)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;
            JSON = json;
        }
        public JsonHandlers JSON
        {
            get => _json ?? new JsonHandlers();
            private set => _json = value;
        }
        public ApplicationRoleManager RoleManager
        {
            get => _roleManager ?? HttpContext.Current.GetOwinContext().Get<ApplicationRoleManager>();
            private set => _roleManager = value;
        }

        public ApplicationSignInManager SignInManager
        {
            get => _signInManager ?? HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>();
            private set => _signInManager = value;
        }

        public ApplicationUserManager UserManager
        {
            get => _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            private set => _userManager = value;
        }
        [Route("api/internalAPI/checkusers", Name = "apicheckusers")]
        [HttpPost, HttpPut, HttpOptions]
        public async Task<HttpResponseMessage> CheckUsers([FromBody]JToken jsonBody)
        {
            var checkUserName = JsonConvert.DeserializeObject<checkUsersClass>(jsonBody.ToString());
            var returnValue = 0;
            var count = false;
            if (!string.IsNullOrEmpty(checkUserName.userId))
            {
                // they are checking a username against everyone else in the system
                var thisUser = await UserManager.FindByIdAsync(checkUserName.userId);
                // now go see if this user is changing anything
                if (checkUserName.username.Contains('@'))
                {
                    if (thisUser.Email != checkUserName.username)
                    {
                        count = UserManager.Users.Any(y => y.Email == checkUserName.username);
                        if (count) returnValue = 1;
                    }
                } else
                {
                    if (thisUser.UserName != checkUserName.username)
                    {
                        count = UserManager.Users.Any(y => y.UserName == checkUserName.username);
                        if (count) returnValue = 1;
                    }
                }
            }
            else
            {
                count = UserManager.Users.Any(y => y.Email.Equals(checkUserName.username));
                if (count) returnValue = 1;
                count = UserManager.Users.Any(y => y.UserName.Equals(checkUserName.username));
                if (count) returnValue = 1;
            }
            var jsonSerializer = new JavaScriptSerializer();
            var json = jsonSerializer.Serialize(returnValue);
            var response = JSON.httpResponse(Request, json);
            return response;
        }
    }

    class checkUsersClass
    {
        public string username { get; set; }
        public string userId { get; set; }
    }
}
