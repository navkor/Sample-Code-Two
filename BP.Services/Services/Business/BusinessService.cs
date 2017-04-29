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

        public async Task<IEnumerable<BusinessViewModel>> PopulateBusinessValues(IEnumerable<BPAccount> businessList, BPMainContext context, IEnumerable<NameStringId> userList)
        {
            var returnList = new ConcurrentBag<BusinessViewModel>();
            await Task.Run(() => Parallel.ForEach(businessList, business => {
               // if (business.LoginAttribute == null) business.LoginAttribute = new LoginAttribute();
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
            }));
            return returnList.OrderBy(x => x.Name);
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

        public async Task<MethodResults> CreateNewBusiness(BusinessPullModel pullModel, BPMainContext context)
        {
            var methodResults = new MethodResults { Success = false, Message = "Something went wrong.  Please try again, or contact your system administrator." };

            // the pull model should have been validated at the controller
            // first, create the account
            var account = new BPAccount() {
                BusinessAttribute = new BusinessAttribute(),
                EntityAttribute = new EntityAttribute()
            };
            account.BusinessAttribute.BusinessName = pullModel.BusinessName;
            account.EntityAttribute.AccountType = await context.AccountTypes.FirstOrDefaultAsync(x => x.Index == 2);
            account.BusinessAttribute.BusinessType = await context.BusinessTypes.FirstOrDefaultAsync(x => x.Index == pullModel.BusinessTypeId);

            context.Accounts.Add(account);

            methodResults = await context.SaveChangesAsync(context);

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
