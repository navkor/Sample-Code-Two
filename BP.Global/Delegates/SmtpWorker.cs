using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BP.Global.Delegates
{
    public class SmtpWorker : IDisposable
    {
        public event EventHandler<SmtpSendEventArgs> EmailSend;
        // Flag: Has Dispose already been called?
        bool disposed = false;
        // Instantiate a SafeHandle instance.
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);
        public void SendEmail(bool success, string purpose)
        {
            OnEmailSend(success, purpose);
        }
        protected virtual void OnEmailSend(bool success, string purpose)
        {
            (EmailSend as EventHandler<SmtpSendEventArgs>)?.Invoke(this, new SmtpSendEventArgs(success, purpose));
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
