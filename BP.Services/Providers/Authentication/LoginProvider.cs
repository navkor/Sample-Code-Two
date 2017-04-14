using Microsoft.Win32.SafeHandles;
using BP.Auth;
using BP.Service.Services.Authentication;
using BP.VM.PullModels.Authentication;
using System;
using System.Linq;
using System.Data.Entity;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using BP.Global.Delegates;
using BP.Global;
using BP.Service.Providers.Logger;
using BP;
using System.Web;

namespace BP.Service.Providers.Authentication
{
    public class LoginProvider : IDisposable
    {
        private BPAuthContext _context;
        private AccountService _service;
        private LoginService _login;
        CoreLoggerProvider _logger;
        AuthenticationWorker _worker;
        CookieProvider _cookie;
        public LoginProvider()
        {
            _service = new AccountService();
            _context = new BPAuthContext();
            _login = new LoginService();
            _logger = new CoreLoggerProvider();
            _worker = new AuthenticationWorker();
            _cookie = new CookieProvider();
        }
        // Flag: Has Dispose already been called?
        bool disposed = false;
        // Instantiate a SafeHandle instance.
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

        public async Task<Registration> LoginAccountStepOne(LoginAccountStep1 pullModel)
        {
            return await _service.AccountLoginStepOne(pullModel, _context);
        }

        public async Task LoginUserFromCookie(Registration registration, string emailAddress)
        {
            var provider = await _context.TokenProviders.FirstOrDefaultAsync(x => x.Index == 10);
            var entity = await _context.ClaimEntities.FirstOrDefaultAsync(x => x.Index == 10);
            string loginToken = await _login.ProcessLoginClaim(registration, provider, entity, _context);
            await _worker_Authentication(new MethodResults {
                Success = true,
                Message = emailAddress,
                ID = registration.ID
            }, "cookie");
        }

        public async Task<AccountLoggedIn> LoginAccountStepTwo(LoginAccountStep2 pullModel, string salt)
        {
            var accountLoggedIn = new AccountLoggedIn();
            var registration = await _service.AccountLoginStepTwo(pullModel, salt, _context);
            string loginToken = "";
            var cookie = new HttpCookie("null");
            var methodResults = new MethodResults {
                Success = false,
                Message = pullModel.Email
            };
            if (registration.ID > 0)
            {
                methodResults.Success = true;
                var provider = await _context.TokenProviders.FirstOrDefaultAsync(x => x.Index == 20);
                var entity = await _context.ClaimEntities.FirstOrDefaultAsync(x => x.Index == 10);
                loginToken = await _login.ProcessLoginClaim(registration, provider, entity, _context);
                cookie = await _cookie.CreateLoginCookieAsync(registration.ID, loginToken, pullModel.RememberMe);
                var session = HttpContext.Current.Session;
                session.SetDataToSession<int>("userID", registration.ID);
            }
            accountLoggedIn = new AccountLoggedIn {
                Registration = registration,
                Cookie = cookie,
                LoginToken = loginToken
            };
            await _worker_Authentication(methodResults, "username/password");

            return accountLoggedIn;
        }

        private async Task _worker_Authentication(MethodResults results, string method)
        {
            var IPAddress = HttpUtility.HtmlEncode (HttpContext.Current.Request.UserHostAddress);
            var message = results.Success ? 
                $"{results.Message} successfully logged in using {method} from {IPAddress}." : 
                $"{results.Message} failed log in using {method} from {IPAddress}.";
            await _logger.CreateNewLog(message, "Account Login", "Login Provider", "Authentication");
        }

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                handle.Dispose();
                _service.Dispose();
                _context.Dispose();
                _worker.Dispose();
                _logger.Dispose();
                _cookie.Dispose();
            }

            // Free any unmanaged objects here.
            //
            disposed = true;
        }
    }
}
