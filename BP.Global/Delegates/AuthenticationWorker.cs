using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BP.Global.Delegates
{
    public class AuthenticationWorker : IDisposable
    {
        public event EventHandler<AuthenticationEventArgs> Authentication;
        // Flag: Has Dispose already been called?
        bool disposed = false;
        // Instantiate a SafeHandle instance.
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

        public void Authenticate(bool success, string username)
        {
            OnAuthenticate(success, username);
        }

        protected virtual void OnAuthenticate(bool success, string username)
        {
            (Authentication as EventHandler<AuthenticationEventArgs>)?.Invoke(this, new AuthenticationEventArgs(success, username));
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
