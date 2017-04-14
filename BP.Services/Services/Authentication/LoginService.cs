using Microsoft.Win32.SafeHandles;
using BP.Auth;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using BP.Service.Providers.Logger;
using BP.Global.Delegates;

namespace BP.Service.Services.Authentication
{
    public class LoginService : IDisposable
    {
        private EncryptionHandler _encrypt;
        CoreLoggerProvider _logger;
        AuthenticationWorker _worker;

        public LoginService()
        {
            _encrypt = new EncryptionHandler();
            _logger = new CoreLoggerProvider();
            _worker = new AuthenticationWorker();
        }
        // Flag: Has Dispose already been called?
        bool disposed = false;
        // Instantiate a SafeHandle instance.
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);
        private Random random = new Random();

        public async Task<string> ProcessLoginClaim(Registration registration, TokenProvider provider, ClaimEntity entity, BPAuthContext context)
        {
            // we have the provider, entity, and registration
            var registerEntry = context.Entry(registration);
            var claim = _encrypt.RandomString(random.Next(8, 15));
            while (context.LoginTokens.Any(y => y.Token.Equals(claim)))
            {
                claim = _encrypt.RandomString(random.Next(8, 15));
            }
            // now we know that no other tokens are like this one
            var loginToken = new LoginToken {
                Token = claim,
                TokenProvider = provider,
                TokenDate = DateTimeOffset.Now
            };
            var dateList = new ConcurrentBag<DateTable>();
            var dateType = await context.ProfileDateTypes
                .FirstOrDefaultAsync(x => x.Index == 10);
            dateList.Add(new DateTable {
                DateLine = DateTimeOffset.Now,
                DateType = dateType
            });
            var tokenClaim = new TokenClaim {
                Claim = registration.IdentityAttribute.UserNames.FirstOrDefault(x => x.NameType.Index == 1000).Name,
                ClaimDates = dateList.ToList(),
                ClaimEntity = entity
            };
            registerEntry.Entity.LoginAttribute.TokenClaims.Add(tokenClaim);
            registerEntry.Entity.LoginAttribute.LoginTokens.Add(loginToken);
            registerEntry.State = EntityState.Modified;
            await context.SaveChangesAsync();
            return claim;
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
                _encrypt.Dispose();
            }

            disposed = true;
        }
    }
}
