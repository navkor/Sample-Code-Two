using Microsoft.Win32.SafeHandles;
using BP.Service.Services.Core;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Text;
using System.Collections.Generic;

namespace BP.Service.Providers.Core
{
    public class SMTPProvider : IDisposable
    {
        SMTPService _service;

        public SMTPProvider()
        {
            Service = new SMTPService();
        }

        public SMTPService Service
        {
            get
            {
                return _service ?? new SMTPService();
            }
            private set
            {
                _service = value;
            }
        }
        public async Task SendNewEmail(string from, string to, string fromName, string toName, string subject, string body, string purpose)
        {
            SendGridMessage message = new SendGridMessage();
            message.SetFrom(new EmailAddress(from, fromName));
            message.AddTo(new EmailAddress(to, toName));
            message.SetSubject(subject);
            message.AddContent(MimeType.Text, body);
            message.AddContent(MimeType.Html, body);
            var tos = new List<string> {
                to
            };
            await Service.SendMailAsync(purpose, message, tos);
        }

        // Flag: Has Dispose already been called?
        bool disposed = false;
        // Instantiate a SafeHandle instance.
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

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
                _service.Dispose();
            }
            disposed = true;
        }
    }
}
