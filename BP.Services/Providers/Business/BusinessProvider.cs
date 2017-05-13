using BP.Main.DataBase;
using BP.Services.Services.Business;
using BP.VM.ViewModels.Business;
using Microsoft.Win32.SafeHandles;
using System;
using System.Data.Entity;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using BP.Service.Providers.Logger;

namespace BP.Services.Providers.Business
{
    public class BusinessProvider : IDisposable
    {
        BPMainContext _context;
        BusinessService _service;
        CoreLoggerProvider _logger;

        public BusinessProvider() { }

        public CoreLoggerProvider Logger
        {
            get => _logger ?? new CoreLoggerProvider();
            private set => _logger = value;
        }

        public BPMainContext Context
        {
            get => _context ?? new BPMainContext();
            private set => _context = value;
        }

        public BusinessService Service
        {
            get => _service ?? new BusinessService();
            private set => _service = value;
        }

        bool disposed = false;
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

        public async Task<BusinessViewModel> BusinessById(int id)
        {
            return await Service.BusinessById(id, Context);
        }

        public async Task<MethodResults> RemoveBusinessAccount(int id, string IPAddress, string instigator)
        {
            return await Service.RemoveBusinessAccount(id, Context, Logger, IPAddress, instigator);
        }

        public async Task<BusinessPullModel> UpdatePullModel(BusinessPullModel pullModel)
        {
            return await Service.UpdatePullModel(pullModel, Context);
        }

        public async Task<IEnumerable<BusinessViewModel>> ViewAllBusinesses(IEnumerable<NameStringId> userList)
        {
            //Context.Configuration.ProxyCreationEnabled = false;
            var coreBusiness = await Context.Accounts.Where(x => x.EntityAttribute.AccountType.Index == 2).ToListAsync();
            var trialBusiness = await Context.Accounts.Where(x => x.EntityAttribute.AccountType.Index == 4).ToListAsync();
            var sampleBusiness = await Context.Accounts.Where(x => x.EntityAttribute.AccountType.Index == 6).ToListAsync();
            var prefinal = coreBusiness.Union(trialBusiness);
            var finalList = prefinal.Union(sampleBusiness);
            // now I have the full list of all businesses in the db
            return Service.PopulateBusinessValues(finalList, Context, userList);

        }

        public async Task<MethodResults> CreateNewBusiness(BusinessPullModel pullModel, string IPAddress, string instigator)
        {
            return await Service.CreateNewBusiness(pullModel, Context, IPAddress, instigator, Logger);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposed)
                return;
            if (disposing)
            {
                handle.Dispose();
                _context.Dispose();
                _service.Dispose();
            }
            disposed = true;
        }
    }
}
