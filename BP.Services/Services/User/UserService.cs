using BP.Global.Models.Main;
using BP.Main.DataBase;
using BP.Service.Providers.Logger;
using Microsoft.Win32.SafeHandles;
using System;
using System.Linq;
using System.Data.Entity;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using BPAccount = BP.Global.Models.Main.Account;
using System.Collections.Generic;

namespace BP.Services.Services.User
{
    public class UserService : IDisposable
    {
        public UserService()
        {

        }

        public async Task<CreateUser> UpdateUserPullModel(CreateUser pullModel, string selectedRole, int selectedAccount, IEnumerable<string> rolesList, BPMainContext context)
        {
            var roleSelectList = new List<NameStringIdSelected>();
            foreach(var role in rolesList)
            {
                roleSelectList.Add(new NameStringIdSelected {
                    ID = role,
                    Name = role,
                    Selected = role.Equals(selectedRole)
                });
            }
            var accountList = await context.Accounts.ToListAsync();
            var accountSelectList = new List<NameIdSelectedLists>();
            if (selectedAccount == 0) accountSelectList.Add(new NameIdSelectedLists { ID = 0, Name = "Account List" });
            foreach(var account in accountList)
            {
                accountSelectList.Add(new NameIdSelectedLists {
                    ID = account.ID,
                    Name = GetAccountName(account, context),
                    Selected = account.ID.Equals(selectedAccount)
                });
            }
            pullModel.Roles = roleSelectList;
            pullModel.AccountLists = accountSelectList;
            return pullModel;
        }

        public async Task<EditUser> UpdateEditPullModel(EditUser pullModel, string selectedRole, int selectedAccount, IEnumerable<string> rolesList, BPMainContext context, string userId)
        {
            var roleSelectList = new List<NameStringIdSelected>();
            foreach (var role in rolesList)
            {
                roleSelectList.Add(new NameStringIdSelected
                {
                    ID = role,
                    Name = role,
                    Selected = role.Equals(selectedRole)
                });
            }
            var accountList = await context.Accounts.ToListAsync();
            var accountSelectList = new List<NameIdSelectedLists>();
            if (selectedAccount == 0) accountSelectList.Add(new NameIdSelectedLists { ID = 0, Name = "Account List" });
            if (selectedAccount < 0)
            {
                if (!string.IsNullOrEmpty(userId))
                {
                    var thisLogin = await context.LoginIds.FirstOrDefaultAsync(x => x.UserId.Equals(userId));
                    if (thisLogin != null)
                    {
                        // there's a login
                        selectedAccount = thisLogin.LoginAttribute.Account.ID;
                    }
                }
            }
            foreach (var account in accountList)
            {
                accountSelectList.Add(new NameIdSelectedLists
                {
                    ID = account.ID,
                    Name = GetAccountName(account, context),
                    Selected = account.ID.Equals(selectedAccount)
                });
            }
            pullModel.Roles = roleSelectList;
            pullModel.AccountLists = accountSelectList;
            return pullModel;
        }

        internal string GetAccountName(BPAccount account, BPMainContext context)
        {
            var created = account.EntityAttribute.AccountDates.FirstOrDefault(x => x.AccountDateType.Index == 1).DateLine.ToString("dd MMM yyyy");
            if (account?.UserAttribute != null)
            {
                if (account?.LoginAttribute != null)
                {
                    // this account actually has login attributes
                    if (account.LoginAttribute.LoginIds.Count > 0)
                    {
                        var loginId = account.LoginAttribute.LoginIds.FirstOrDefault(x => x.PresidenceType.Index == 1);
                        return $"{account.ID} {loginId.EmailAddress} created {created}";
                    }
                }
                return $"{account.ID} User account created {created}";
            }
            if (!string.IsNullOrEmpty(account?.BusinessAttribute?.BusinessName ?? "")) return $"{account.ID} {account.BusinessAttribute.BusinessName ?? ""} created {created}";
            return $"{account.ID} Business account created {created}";
        }

        public async Task<MethodResults> AddUserToAccount(string userId, string emailAddress, string userName, CoreLoggerProvider logger, string IPAddress, string instigator, BPMainContext context, int accountId, int loginTypeId, int presidenceTypeId)
        {
            var methodResults = new MethodResults { Success = false, Message = "Something went wrong.  Nothing was changed.  Please try again or contact your network or system administrator." };
            // we're adding a user to either an existing account, or to a new account
            var account = new BPAccount();
            if (accountId == 0)
            {
                var dateType = await context.AccountDateTypes.FirstOrDefaultAsync(x => x.Index == 1);
                var accountType = await context.AccountTypes.FirstOrDefaultAsync(x => x.Index == 1);
                var accountDates = new List<AccountDate> {
                    new AccountDate { DateLine = DateTimeOffset.Now, AccountDateType = dateType }
                };
            
                account.EntityAttribute = new EntityAttribute
                {
                    AccountDates = accountDates,
                    AccountType = accountType
                };
                account.UserAttribute = new UserAttribute();
            }
            else account = await context.Accounts.FindAsync(accountId);
            var loginType = await context.LoginIdTypes.FirstOrDefaultAsync(x => x.Index == loginTypeId);
            var presidenceType = await context.PresidenceTypes.FirstOrDefaultAsync(x => x.Index == presidenceTypeId);
            var loginIds = new List<LoginId> {
                new LoginId {
                    UserId = userId,
                    EmailAddress = emailAddress,
                    PresidenceType = presidenceType,
                    LoginIdType = loginType
                }
            };
            var loginAttribute = new LoginAttribute
            {
                LoginIds = loginIds
            };
            if (accountId > 0)
            {
                var entry = context.Entry(account);
                entry.Entity.LoginAttribute = loginAttribute;
                entry.State = EntityState.Modified;
            }
            else {
                account.LoginAttribute = loginAttribute;
                context.Accounts.Add(account);
            }
            methodResults = await context.SaveChangesAsync(context);
            var subject = "Add user to account";
            var system = "User Service";
            if (methodResults.Success) await logger.CreateNewLog($"Successfully added user {emailAddress} to account {account.ID} on {IPAddress} by {instigator}.", subject, instigator, system);
            else await logger.CreateNewLog($"Unable to add user {emailAddress} to an account on {IPAddress} by {instigator}.", subject, instigator, system);
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
            }
            disposed = true;
        }
    }
}
