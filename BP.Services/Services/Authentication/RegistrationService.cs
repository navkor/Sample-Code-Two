using Microsoft.Win32.SafeHandles;
using BP.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BP.Service.Services.Authentication
{
    public class RegistrationService : IDisposable
    {
        // Flag: Has Dispose already been called?
        bool disposed = false;
        // Instantiate a SafeHandle instance.
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);
        public async Task<bool> IsUserNameGood(string userName, BPAuthContext context)
        {
            var returnValue = true;
            await Task.Run(() => Parallel.ForEach(context.UserNames.Where(x => x.NameType.Index == 1000), un => {
                if (un.Name.ToLower().Equals(userName.ToLower()))
                {
                    returnValue = false;
                }
            }));
            return returnValue;
        }
        public async Task<bool> IsEmailGood(string email, BPAuthContext context)
        {
            var returnValue = true;
            await Task.Run(() => Parallel.ForEach(context.EmailAddresses, em => {
                if (em.Email.Equals(email)) returnValue = false;
            }));
            return returnValue;
        }
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
