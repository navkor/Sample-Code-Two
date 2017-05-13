using BP.Main.DataBase;
using BP.Service.Providers.Logger;
using BP.Services.Services.User;
using BP.Web;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace BP.Services.Providers.User
{
    public class UserProvider : IDisposable
    {
        UserService _service;
        BPMainContext _context;
        CoreLoggerProvider _logger;
        ApplicationUserManager _userManager;
        public UserProvider() { }
        public UserProvider(ApplicationUserManager userManager)
        {
            UserManager = userManager;

        }

        public ApplicationUserManager UserManager
        {
            get => _userManager;
            private set => _userManager = value;
        }

        public BPMainContext Context
        {
            get => _context ?? new BPMainContext();
            private set => _context = value;
        }

        public CoreLoggerProvider Logger
        {
            get => _logger ?? new CoreLoggerProvider();
            private set => _logger = value;
        }

        public UserService Service
        {
            get => _service ?? new UserService();
            private set => _service = value;
        }

        public async Task<CreateUser> UpdateUserPullModel(CreateUser pullModel, string selectedRole, int selectedAccount, IEnumerable<string> rolesList)
        {
            return await Service.UpdateUserPullModel(pullModel, selectedRole, selectedAccount, rolesList, Context);
        }

        public async Task<EditUser> UpdateEditPullModel(EditUser pullModel, string selectedRole, int selectedAccount, IEnumerable<string> rolesList, string userId)
        {
            return await Service.UpdateEditPullModel(pullModel, selectedRole, selectedAccount, rolesList, Context, userId);
        }
        /*
            userId = the IP of the Authentication account being added
            emailAddress = the emailAddress of the user to be added
            userName = the userName of the user to be added
            instigator = The account of the person doing this addition
            IPAddress = The IPAddress of the network the instigator is user
            accountId = The account this user is being assigned to (could be a new account which means pass a 0)
            loginTypeId = The type of login of this user account (business, user, owner, guest, etc)
            presidenceTypeId = The type of user for this account (Owner, primary, etc)
        */
        public async Task<MethodResults> AddUserToAccount(string userId, string emailAddress, string userName, string instigator, string IPAddress, int accountId, int loginTypeId, int presidenceTypeId)
        {
            var methodResults = await Service.AddUserToAccount(userId, emailAddress, userName, Logger, IPAddress, instigator, Context, accountId, loginTypeId, presidenceTypeId);
            return methodResults;
        }

        // Flag: Has Dispose already been called?
        bool disposed = false;
        // Instantiate a SafeHandle instance.
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

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
                if (_service != null)
                {
                    _service.Dispose();
                    _service = null;
                }
                if (_context != null)
                {
                    _context.Dispose();
                    _context = null;
                }
                if (_logger != null)
                {
                    _logger.Dispose();
                    _logger = null;
                }
            }
            disposed = true;
        }
    }
}
