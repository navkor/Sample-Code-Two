using Microsoft.Win32.SafeHandles;
using BP.Auth;
using BP.Service.Services.Authentication;
using BP.VM.PullModels.Authentication;
using System;
using System.Data.Entity;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using BP.Global.Delegates;
using BP.Service.Providers.Logger;
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
            return await _service.AccountLoginStepOne(pullModel, _context).ConfigureAwait(continueOnCapturedContext: false);
        }

        public async Task LoginUserFromCookie(Registration registration, string emailAddress, string IPAddress)
        {
            var provider = await _context.TokenProviders.FirstOrDefaultAsync(x => x.Index == 10).ConfigureAwait(continueOnCapturedContext: false);
            var entity = await _context.ClaimEntities.FirstOrDefaultAsync(x => x.Index == 10).ConfigureAwait(continueOnCapturedContext: false);
            string loginToken = await _login.ProcessLoginClaim(registration, provider, entity, _context).ConfigureAwait(continueOnCapturedContext: false);
            await _worker_Authentication(new MethodResults {
                Success = true,
                Message = emailAddress,
                ID = registration.ID
            }, "cookie", IPAddress).ConfigureAwait(continueOnCapturedContext: false);
        }

        public async Task<AccountLoggedIn> LoginAccountStepTwo(LoginAccountStep2 pullModel, string salt, HttpSessionStateBase session, string IPAddress)
        {
            var accountLoggedIn = new AccountLoggedIn();
            var registration = await _service.AccountLoginStepTwo(pullModel, salt, _context).ConfigureAwait(continueOnCapturedContext: false);
            string loginToken = "";
            var cookie = new HttpCookie("null");
            var methodResults = new MethodResults {
                Success = false,
                Message = pullModel.Email
            };
            if (registration.ID > 0)
            {
                methodResults.Success = true;
                var provider = await _context.TokenProviders.FirstOrDefaultAsync(x => x.Index == 20).ConfigureAwait(continueOnCapturedContext: false);
                var entity = await _context.ClaimEntities.FirstOrDefaultAsync(x => x.Index == 10).ConfigureAwait(continueOnCapturedContext: false);
                loginToken = await _login.ProcessLoginClaim(registration, provider, entity, _context).ConfigureAwait(continueOnCapturedContext: false);
                cookie = await _cookie.CreateLoginCookieAsync(registration.ID, loginToken, pullModel.RememberMe).ConfigureAwait(continueOnCapturedContext: false);
                session.SetDataToSession<int>("userID", registration.ID);
            }
            accountLoggedIn = new AccountLoggedIn {
                Registration = registration,
                Cookie = cookie,
                LoginToken = loginToken
            };
            await _worker_Authentication(methodResults, "username/password", IPAddress);

            return accountLoggedIn;
        }

        private async Task _worker_Authentication(MethodResults results, string method, string IPAddress)
        {
            var message = results.Success ? 
                $"{results.Message} successfully logged in using {method} from {IPAddress}." : 
                $"{results.Message} failed log in using {method} from {IPAddress}.";
            await _logger.CreateNewLog(message, "Account Login", "Login Provider", "Authentication").ConfigureAwait(continueOnCapturedContext: false);
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
