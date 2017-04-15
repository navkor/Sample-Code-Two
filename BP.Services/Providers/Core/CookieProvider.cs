using Microsoft.Win32.SafeHandles;
using BP.Auth;
using BP.VM.ViewModels.Authentication;
using System;
using System.Runtime.InteropServices;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using BP.Service.Services.Core;
using System.Web;
using BP.VM.PullModels.Authentication;

namespace BP.Service.Providers.Authentication
{
    public class CookieProvider : IDisposable
    {
        Random random = new Random();
        string cookiePrefix = "u3e54iwerskli_cosok_1rr8_";
        JavaScriptSerializer script;
        BPAuthContext _context;
        CookieService _service;
        public CookieProvider()
        {
            _context = new BPAuthContext();
            script = new JavaScriptSerializer();
            _service = new CookieService();
        }
        // Flag: Has Dispose already been called?
        bool disposed = false;
        // Instantiate a SafeHandle instance.
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

        public async Task<HttpCookie> CreateLoginCookieAsync(int userID, string token, bool rememberMe)
        {
            var cookieViewModel = new CookieLoginViewModel {
                userID = userID,
                Token = token,
                RememberMe = rememberMe
            };
            var cookie = new HttpCookie("null");
            var cookieType = await _context.CookieTypes.FirstOrDefaultAsync(x => x.Index == 100);
            var data = script.Serialize(cookieViewModel);
            var expiration = rememberMe ?
                DateTimeOffset.Now.AddDays(14) :
                DateTimeOffset.Now.AddDays(1);
            var pullModel = await _service.CreateCookieDataInstanceAsync(data, expiration, cookieType, _context);
            if (pullModel.ID > 0)
            {
                cookie = CreateCookie("login", expiration, pullModel);
            }
            return cookie;
        }

        public async Task<CookieLoginViewModel> RetrieveLoginCookie()
        {
            var cookieLoginVM = new CookieLoginViewModel();
            // first, let's see if the cookie even exists
            var cookie = HttpContext.Current.Request.Cookies[$"{cookiePrefix}login"];
            if (cookie != null)
            {
                // let's make sure it's not expired
                var dateDiff = new TimeSpan();
                dateDiff = cookie.Expires - DateTime.Now;
                if (dateDiff.Days > 0)
                {
                    var id = cookie["ID"];
                    var code = cookie["Code"];
                    if (!string.IsNullOrEmpty(id))
                    {
                        if (!string.IsNullOrEmpty(code))
                        {
                            // we have everything that we need to continue
                            var cookieDB = await _service.RetrieveCookieFromDatabase(int.Parse(id), code, _context);
                            if (cookieDB.ID > 0)
                            {
                                cookieLoginVM = script.Deserialize<CookieLoginViewModel>(cookieDB.Data);
                            }
                        }
                    }
                }
            }
            return cookieLoginVM;
        }

        public HttpCookie RemoveLoginCOokie()
        {
            return RemoveCookie("login");
        }

        public HttpCookie CreateCookie(string name, DateTimeOffset expiration, CookiePullModel pullModel)
        {
            var cookie = new HttpCookie($"{cookiePrefix}{name}");
            cookie["ID"] = pullModel.ID.ToString();
            cookie["Code"] = pullModel.Code;
            cookie.Expires = new DateTime(expiration.Year, expiration.Month, expiration.Day);
            return cookie;
        }

        public HttpCookie RemoveCookie(string name)
        {
            var cookie = HttpContext.Current.Request.Cookies[$"{cookiePrefix}{name}"];
            cookie.Expires = DateTime.Now.AddDays(-1);
            return cookie;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;
            if (disposing)
            {
                handle.Dispose();
                _context.Dispose();
                _service.Dispose();
            }
            disposed = true;
        }
    }
}
