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

namespace BP.Services.Providers.Business
{
    public class BusinessProvider : IDisposable
    {
        BPMainContext _context;
        BusinessService _service;

        public BusinessProvider() { }
        public BusinessProvider(BPMainContext context) : this(context, null) { }
        public BusinessProvider(BusinessService service) : this(null, service) { }

        public BusinessProvider(BPMainContext context, BusinessService service)
        {
            _context = context;
            _service = service;
        }

        public BPMainContext Context
        {
            get
            {
                return _context ?? new BPMainContext();
            }
            private set
            {
                _context = value;
            }
        }

        public BusinessService Service
        {
            get
            {
                return _service ?? new BusinessService();
            }
            private set
            {
                _service = value;
            }
        }

        bool disposed = false;
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

        public async Task<BusinessPullModel> UpdatePullModel(BusinessPullModel pullModel)
        {
            return await Service.UpdatePullModel(pullModel, Context);
        }

        public async Task<IEnumerable<BusinessViewModel>> ViewAllBusinesses(IEnumerable<NameStringId> userList)
        {
            var coreBusiness = await Context.Accounts.Where(x => x.EntityAttribute.AccountType.Index == 2).ToListAsync();
            var trialBusiness = await Context.Accounts.Where(x => x.EntityAttribute.AccountType.Index == 4).ToListAsync();
            var sampleBusiness = await Context.Accounts.Where(x => x.EntityAttribute.AccountType.Index == 6).ToListAsync();
            var prefinal = coreBusiness.Union(trialBusiness);
            var finalList = prefinal.Union(sampleBusiness);
            // now I have the full list of all businesses in the db
            return await Service.PopulateBusinessValues(finalList, Context, userList);

        }

        public async Task<MethodResults> CreateNewBusiness(BusinessPullModel pullModel)
        {
            return await Service.CreateNewBusiness(pullModel, Context);
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
