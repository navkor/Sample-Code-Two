using BP.Global.Models.Main;
using BP.Main.DataBase;
using BP.VM.ViewModels.Business;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<IEnumerable<BusinessViewModel>> PopulateBusinessValues(IEnumerable<Account> businessList, BPMainContext context)
        {
            var returnList = new ConcurrentBag<BusinessViewModel>();
            var businessType = new BusinessType();
            await Task.Run(() => Parallel.ForEach(businessList, business => {
                businessType = business.BusinessAttribute.BusinessType;
                returnList.Add(new BusinessViewModel
                {
                    BusinessType = new NameIdLists
                    {
                        ID = businessType.ID,
                        Name = businessType.Name
                    },
                    ID = business.ID,
                    Name = business.BusinessAttribute.BusinessName,
                    Logins = business.LoginAttribute.LoginIds.Select(x => new NameIdValueNoteLists
                    {
                        ID = x.ID,
                        Value = x.LoginIdType.Name,
                        Note = x.UserId
                    }).ToList()
                });
            }));
            return returnList;
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
