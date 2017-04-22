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

        public BusinessProvider()
        {
            _context = new BPMainContext();
            _service = new BusinessService();
        }

        bool disposed = false;
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

        public async Task<IEnumerable<BusinessViewModel>> ViewAllBusinesses()
        {
            var coreBusiness = await _context.Accounts.Where(x => x.EntityAttribute.AccountType.Index == 2).ToListAsync();
            var trialBusiness = await _context.Accounts.Where(x => x.EntityAttribute.AccountType.Index == 4).ToListAsync();
            var sampleBusiness = await _context.Accounts.Where(x => x.EntityAttribute.AccountType.Index == 6).ToListAsync();
            var prefinal = coreBusiness.Union(trialBusiness);
            var finalList = prefinal.Union(sampleBusiness);
            // now I have the full list of all businesses in the db
            return await _service.PopulateBusinessValues(finalList, _context);

        }

        public async Task<MethodResults> CreateNewBusiness(BusinessPullModel pullModel)
        {
            var methodResults = new MethodResults {
                Success = false,
                Message = "Something has gone wrong.  Please try again"
            };

            return methodResults;
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
