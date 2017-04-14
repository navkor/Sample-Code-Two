using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Linq;
using System.Data.Entity;
using BP.VM.PullModels.Authentication;
using BP.Auth;

namespace BP.Service.Services.Core
{
    public class CookieService : IDisposable
    {
        Random random = new Random();
        // Flag: Has Dispose already been called?
        bool disposed = false;
        // Instantiate a SafeHandle instance.
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);
        public async Task<CookiePullModel> CreateCookieDataInstanceAsync(string data, DateTimeOffset expiration, CookieType cookieType, BPAuthContext context)
        {
            var cookieCode = "";
            var pullModel = new CookiePullModel();

            using (var encrypt = new EncryptionHandler())
            {
                cookieCode = encrypt.RandomString(random.Next(20, 50));
            }
            pullModel.Code = cookieCode;
            pullModel.Expiration = expiration;
            // now let's create this cookie
            var cookie = new Cookie {
                Code = cookieCode,
                Data = data,
                DateSet = DateTimeOffset.Now,
                Expiration = expiration,
                CookieType = cookieType
            };

            context.Cookies.Add(cookie);
            var methodResults = await context.SaveChangesAsync(context);
            if (methodResults.Success) pullModel.ID = cookie.ID;
            else return new CookiePullModel();

            return pullModel;
        }

        public async Task<Cookie> RetrieveCookieFromDatabase(int id, string code, BPAuthContext context)
        {
            var cookie = new Cookie();

            cookie = await context.Cookies.FirstOrDefaultAsync(y => y.ID == id && y.Code == code); 

            return cookie;
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
