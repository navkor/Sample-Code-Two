using BP.Global.Models.Main;
using BP.Main.DataBase;
using BP.VM.ViewModels.Business;
using Microsoft.Win32.SafeHandles;
using System;
using System.Data.Entity;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using BPAccount = BP.Global.Models.Main.Account;
using System.Web.Mvc;
using BP.Service.Providers.Logger;

namespace BP.Services.Services.Business
{
    public class BusinessService : IDisposable
    {

        bool disposed = false;
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IEnumerable<BusinessViewModel> PopulateBusinessValues(IEnumerable<BPAccount> businessList, BPMainContext context, IEnumerable<NameStringId> userList)
        {
            var returnList = new List<BusinessViewModel>();
            foreach(var business in businessList)
            {
                returnList.Add(new BusinessViewModel
                {
                    BusinessType = new NameIdLists
                    {
                        ID = business.BusinessAttribute.BusinessType.ID,
                        Name = business.BusinessAttribute.BusinessType.Name
                    },
                    ID = business.ID,
                    Name = business.BusinessAttribute.BusinessName,
                    Logins = business?.LoginAttribute?.LoginIds.Select(x => new NameIdValueNoteLists
                    {
                        ID = x.ID,
                        Value = x.LoginIdType.Name,
                        Note = x.UserId,
                        Index = x.LoginIdType.Index,
                        Name = userList.FirstOrDefault(y => y.ID == x.UserId).Name
                    }).OrderBy(n => n.Index).ToList() ?? new List<NameIdValueNoteLists>()
                });
            }
            return returnList.OrderBy(x => x.Name);
        }

        public async Task<BusinessViewModel> BusinessById(int id, BPMainContext context)
        {
            // we're going to get the business by id
            var business = await context.Accounts.FindAsync(id);
            if (business == null) return new BusinessViewModel();
            // This business actually exists!
            var returnModel = new BusinessViewModel {
                ID = business.ID,
                Name = business.BusinessAttribute.BusinessName,
                BusinessType = new NameIdLists {
                    ID = business.BusinessAttribute.BusinessType.ID,
                    Name = business.BusinessAttribute.BusinessType.Name },
                Logins = business?.LoginAttribute?.LoginIds.Select(y => new NameIdValueNoteLists {
                    ID = y.ID,
                    Name = y.UserId
                }) ?? new List<NameIdValueNoteLists>()
            };
            return returnModel;
        }

        internal async Task<BusinessPullModel> UpdatePullModel(BusinessPullModel pullModel, BPMainContext context)
        {
            var businessTypeList = new List<SelectListItem>();
            if (pullModel.BusinessTypeId == 0)
            {
                businessTypeList.Add(new SelectListItem {
                    Text = "Select a business type...",
                    Value = "0"
                });
            }
            businessTypeList.AddRange(await context.BusinessTypes.Select(y => new SelectListItem {
                Text = y.Name,
                Value = y.ID.ToString(),
                Selected = y.ID == pullModel.BusinessTypeId
            }).ToListAsync());

            pullModel.BusinessTypes = businessTypeList;
            return pullModel;
        }

        public async Task<MethodResults> RemoveBusinessAccount(int id, BPMainContext context, CoreLoggerProvider logger, string IPAddress, string instigator)
        {
            // we're removing this business from the website
            var account = await context.Accounts.FindAsync(id);
            if (account == null) return new MethodResults { Success = false, Message = "This business could not be deleted because it could not be found." };
            var subject = "Account removal";
            var system = "Business Service";
            var businessName = account.BusinessAttribute.BusinessName;
            if (account.LoginAttribute != null)
            {
                context.LoginAttributes.Remove(account.LoginAttribute);
            }
            if (account.EntityAttribute != null) context.EntityAttributes.Remove(account.EntityAttribute);
            if (account.BusinessAttribute != null) context.BusinessAttributes.Remove(account.BusinessAttribute);
            if (account.UserAttribute != null) context.UserAttributes.Remove(account.UserAttribute);
            context.Accounts.Remove(account);
            var returnResults = await context.SaveChangesAsync(context);
            if (returnResults.Success) await logger.CreateNewLog($"Business account {businessName} successfully removed by {instigator} on {IPAddress}", subject, instigator, system);
            else await logger.CreateNewLog($"Could not remove business account {businessName} by {instigator} on {IPAddress}", subject, instigator, system);
            return returnResults;
        }

        public async Task<MethodResults> CreateNewBusiness(BusinessPullModel pullModel, BPMainContext context, string IPAddress, string instigator, CoreLoggerProvider logger)
        {
            var methodResults = new MethodResults { Success = false, Message = "Something went wrong.  Please try again, or contact your system administrator." };

            // the pull model should have been validated at the controller
            // first, create the account
            var account = new BPAccount() {
                BusinessAttribute = new BusinessAttribute(),
                EntityAttribute = new EntityAttribute()
            };
            account.BusinessAttribute.BusinessName = pullModel.BusinessName;
            var subject = "Business Account Creation";
            var system = "Business Service";
            account.EntityAttribute.AccountType = await context.AccountTypes.FirstOrDefaultAsync(x => x.Index == 2);
            account.BusinessAttribute.BusinessType = await context.BusinessTypes.FirstOrDefaultAsync(x => x.Index == pullModel.BusinessTypeId);
            var accountDateType = await context.AccountDateTypes.FirstOrDefaultAsync(x => x.Index == 1);
            var accountDate = new AccountDate {
                AccountDateType = accountDateType,
                DateLine = DateTimeOffset.Now
            };
            account.EntityAttribute.AccountDates.Add(accountDate);

            context.Accounts.Add(account);

            methodResults = await context.SaveChangesAsync(context);
            if (methodResults.Success) await logger.CreateNewLog($"Successfully create {pullModel.BusinessName} as {pullModel.BusinessTypes.FirstOrDefault(x => x.Value == pullModel.BusinessTypeId.ToString())} by {instigator} on {IPAddress}", subject, instigator, system);
            else await logger.CreateNewLog($"Failed to create new business {pullModel.BusinessName} by {instigator} on {IPAddress}", subject, instigator, system);
            return methodResults;
        }

        private void Dispose(bool disposing)
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
