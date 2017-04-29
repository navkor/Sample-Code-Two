using BP.Main.DataBase;
using BP.VM.ViewModels.AccountAdmin;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Linq;
using System.Data.Entity;
using BPAccount = BP.Global.Models.Main.Account;
using BP.Web;
using System.Collections.Concurrent;

namespace BP.Services.Services.Account
{
    public class AccountService : IDisposable
    {
        public AccountService()
        {      }
        

        public async Task<IEnumerable<AccountAdminViewModel>> ReturnAccountList(int returnFirst, bool byName, bool byDate, int sorted, BPMainContext context, ApplicationUserManager manager)
        {
            IEnumerable<AccountAdminViewModel> returnList = new List<AccountAdminViewModel>();
            if (byName)
            {
                if (byDate)
                {

                }
            }
            if (sorted > 0)
            {

            }
            return await AccountFirstN(returnFirst, context, manager);
        }

        private async Task<IEnumerable<AccountAdminViewModel>> AccountFirstN(int returnFirst, BPMainContext context, ApplicationUserManager manager)
        {
            var checkList = await PrepopulateAccountsWithUserName(context, manager);
            if (returnFirst > 0) return checkList.OrderBy(x => x.ID).Take(returnFirst);
            return checkList.OrderBy(x => x.ID);
        }

        private async Task<IEnumerable<AccountAdminViewModel>> AccountByName(int returnFirst, int sorted, BPMainContext context, ApplicationUserManager manager)
        {
            var checkList = await PrepopulateAccountsWithUserName(context, manager);
            if (sorted > 0)
            {
                switch (sorted) {
                    case 1:
                        checkList = checkList.OrderBy(x => x.UserName);
                        break;
                    case 2:
                        checkList = checkList.OrderByDescending(x => x.UserName);
                        break;
                    default:
                        checkList = checkList.OrderBy(x => x.ID);
                        break;
                }
            }
            if (returnFirst > 0) return checkList.Take(returnFirst);
            return checkList;
        }

        private async Task<IEnumerable<AccountAdminViewModel>> PrepopulateAccountsWithUserName(BPMainContext context, ApplicationUserManager manager)
        {
            var list = await context.Accounts.ToListAsync();
            var returnList = new ConcurrentBag<AccountAdminViewModel>();
            var userList = await manager.Users.ToListAsync();
            await Task.Run(() => Parallel.ForEach(list, account => {
                // we have the list of accounts, let's go find out if they have accounts associated with them
                var accountAdminViewModel = new AccountAdminViewModel();
                accountAdminViewModel.ID = account.ID;
                if (account.LoginAttribute != null)
                {
                    var loginId = account.LoginAttribute.LoginIds.FirstOrDefault(x => x.LoginIdType.Index == 4);
                    if (loginId != null)
                    {
                        // we're all set for this account
                        var user = userList.FirstOrDefault(x => x.Id.Equals(loginId.UserId));
                        if (user != null)
                        {
                            accountAdminViewModel.UserName = user.UserName;
                            accountAdminViewModel.EmailAddress = user.Email;
                        }
                    }
                }
                if (account.BusinessAttribute != null) accountAdminViewModel.AccountType = "Business";
                else if (account.UserAttribute != null) accountAdminViewModel.AccountType = "Personal";
                else accountAdminViewModel.AccountType = "UnOwned";
                returnList.Add(accountAdminViewModel);
            }));
            return returnList;
        }

        private async Task<IEnumerable<AccountAdminViewModel>> Generate(IEnumerable<BPAccount> accountList, BPMainContext context)
        {
            IEnumerable<AccountAdminViewModel> returnList = new List<AccountAdminViewModel>();

            return returnList;
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
            }
            disposed = true;
        }
    }
}
