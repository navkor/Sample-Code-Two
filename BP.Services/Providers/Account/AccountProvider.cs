using BP.Main.DataBase;
using BP.Service.Providers.Logger;
using BP.Services.Services.Account;
using BP.VM.ViewModels.AccountAdmin;
using BP.Web;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace BP.Services.Providers.Account
{
    public class AccountProvider : IDisposable
    {
        AccountService _service;
        BPMainContext _context;
        CoreLoggerProvider _logger;
        ApplicationUserManager _userManager;
        public AccountProvider() { }
        public AccountProvider(ApplicationUserManager userManager)
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

        public AccountService Service
        {
            get => _service ?? new AccountService();
            private set => _service = value;
        }

        public async Task<IEnumerable<AccountAdminViewModel>> ReturnFirstNAccounts(int returnFirst)
        {
            return await Service.ReturnAccountList(returnFirst, false, false, 0, Context, UserManager);
        }

        public async Task<IEnumerable<AccountAdminViewModel>> ReturnFirstNByName(int returnFirst, bool SortUp)
        {
            var sorted = SortUp ? 1 : 2;
            return await Service.ReturnAccountList(returnFirst, true, false, sorted, Context, UserManager);
        }

        public async Task<IEnumerable<AccountAdminViewModel>> ReturnSortedAccountList(int returnFirst, bool byName, bool byDate, int sorted)
        {
            return await Service.ReturnAccountList(returnFirst, byName, byDate, sorted, Context, UserManager);
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
