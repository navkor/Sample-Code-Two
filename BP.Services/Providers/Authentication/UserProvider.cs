using Microsoft.Win32.SafeHandles;
using BP.Auth;
using BP.Service.Providers.Logger;
using BP.VM.ViewModels.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BP.Service.Providers.Authentication
{
    /*
     * this class will provide user authentication retrieval
     * If a user is logged in, this class will be the one to globally expose that credential
     
        */
    public class UserProvider : IDisposable
    {
        BPAuthContext _context;
        CoreLoggerProvider _logger;
        CookieProvider _cookie;
        LoginProvider _login;

        public UserProvider()
        {
            _context = new BPAuthContext();
            _login = new LoginProvider();
            _logger = new CoreLoggerProvider();
            _cookie = new CookieProvider();
        }
        // Flag: Has Dispose already been called?
        bool disposed = false;
        // Instantiate a SafeHandle instance.
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

        /*
            So, let's return a logged in user, shall we 
        */
        public async Task<LoggedInUserVM> LoggedInUser()
        {
            var vm = new LoggedInUserVM();
            // first, let's see if this user is in the session
            var session = HttpContext.Current.Session;
            var registration = new Registration();
            if (session.Count > 0)
            {
                // now, let's go get the userID
                int userID = 0;
                if (int.TryParse(session.GetDataFromSession<int>("userID").ToString(), out userID))
                {
                    if (userID > 0)
                    {
                        registration = await _context.Registrations.FindAsync(userID);
                        var user =  retrieveViewModel(registration);
                        return user;
                    }
                }
            }
            // then, let's see if there is a cookie
            var cookieLVM = await _cookie.RetrieveLoginCookie();
            if (cookieLVM.userID > 0)
            {
                // this user does have a log in cookie and is able to login using it
                // so, now we need to log them in as a cookie log in rather than a regular log in
                registration = await _context.Registrations.FindAsync(cookieLVM.userID);
                vm = retrieveViewModel(registration);
                await _login.LoginUserFromCookie(registration, vm.EmailAddress);
            }

            return vm;
        }

        internal LoggedInUserVM retrieveViewModel(Registration registration)
        {
            var vm = new LoggedInUserVM();
            return vm = new LoggedInUserVM
            {
                EmailAddress = RetrieveEmailAddress(registration),
                RoleIndex = registration.AccountAttribute.UserRole.Index,
                UserName = registration.IdentityAttribute?.UserNames?.FirstOrDefault(y => y.NameType.Index == 1000).Name ?? "",
                UserID = registration.ID,
                UserRole = registration.AccountAttribute.UserRole.Name
            };
            return vm;
        }

        internal string RetrieveEmailAddress (Registration registration)
        {
            // first, let' see if there is a default email address
            if (registration.ProfileAttribute.EmailAddresses.Any(y => y.Default))
                return registration.ProfileAttribute.EmailAddresses.FirstOrDefault(y => y.Default == true).Email;
            return registration.ProfileAttribute.EmailAddresses.FirstOrDefault(y => y.Validated).Email;
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
                _context.Dispose();
                _logger.Dispose();
                _cookie.Dispose();
                _login.Dispose();
            }

            // Free any unmanaged objects here.
            //
            disposed = true;
        }
    }
}
