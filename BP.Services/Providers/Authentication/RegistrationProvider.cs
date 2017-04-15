using Microsoft.Win32.SafeHandles;
using BP.Auth;
using BP.Global.Delegates;
using BP.Service.Providers.Logger;
using BP.Service.Services.Authentication;
using BP.VM.PullModels.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BP.Service.Providers.Authentication
{
    public class RegistrationProvider : IDisposable
    {
        private BPAuthContext _context;
        private RegistrationService _service;
        private AccountService _account;
        string instigator = "Registration Provider";
        string system = "Authentication";
        CoreLoggerProvider _logger;
        AuthenticationWorker _worker;

        public RegistrationProvider()
        {
            _context = new BPAuthContext();
            _service = new RegistrationService();
            _account = new AccountService();
            _worker = new AuthenticationWorker();
            _logger = new CoreLoggerProvider();
        }
        // Flag: Has Dispose already been called?
        bool disposed = false;
        // Instantiate a SafeHandle instance.
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

        // this will allow the person to look up and see if they can register for an account or not
        // it will check against current fields and see if that information has already been used for previous registrations
        public async Task<MethodResults> RegisteringAccount(RegisterAccount pullModel)
        {
            var userName = pullModel.Email;
            var methodResults = FailedMessage("Registering an account", "something went wrong");
            // first, we need to find out if the information has laready been used to register
            if (!pullModel.Email.Equals(pullModel.ConfirmEmail)) return FailedMessage("Registering an account", "emails do not match");
            if (await _service.IsEmailGood(pullModel.Email, _context) == false) return FailedMessage("Registering an account", "email cannot be used");
            if (await _service.IsUserNameGood(pullModel.Email, _context) == false) return FailedMessage("Registering an account", "email cannot be used");
            // everything is good so far
            // so let's go ahead and press forward
            var registration = await _account.RegisterNewStandardAccount(pullModel, _context);
            var validationString = registration?.ProfileAttribute?.EmailAddresses?.FirstOrDefault()?.ValidationString ?? "";
            _context.Registrations.Add(registration);
            methodResults = await _context.SaveChangesAsync(_context);
            if (methodResults.Success)
            {
                methodResults.Message = validationString;
                methodResults.ID = registration.ID;
            }
            await _worker_Authentication(pullModel.Email, methodResults);
            return methodResults;
        }

        public async Task<MethodResults> ActivateAccount(ActivateEmailModel model, string IPAddress)
        {
            var methodResults = await _account.ActivateAccount(model, _context);
            await WorkerLogAttemptActivate(methodResults, IPAddress);
            return methodResults;
        }

        public async Task WorkerLogAttemptActivate(MethodResults results, string IPAddress)
        {
            var subject = "Account Activation";
            var message = results.Success ?
                $"Successful account activation for {results.Message} on {IPAddress}" :
                $"Unsuccessful activation attempt for userID {results.ID} on {IPAddress}";
            await _logger.CreateNewLog(message, subject, instigator, system);
        }

        private async Task _worker_Authentication(string userName, MethodResults results)
        {
            var subject = "New Account Registration";
            var IPAddress = System.Web.HttpUtility.HtmlEncode(System.Web.HttpContext.Current.Request.UserHostAddress);
            var message = results.Success ?
                $"{userName} Registered successfully from {IPAddress}." :
                $"{userName} registration failed. from {IPAddress}";
            await _logger.CreateNewLog(message, subject, instigator, system);
        }
        

        private MethodResults FailedMessage(string action, string reason)
        {
            return new MethodResults { Success = false, ID = -1, Message = $"{action} could not be completed because {reason}." };
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
                _service.Dispose();
                _account.Dispose();
                _worker.Dispose();
                _logger.Dispose();
            }

            // Free any unmanaged objects here.
            //
            disposed = true;
        }
    }
}
