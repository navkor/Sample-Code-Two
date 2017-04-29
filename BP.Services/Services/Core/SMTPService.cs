using Microsoft.Win32.SafeHandles;
using BP.Service.Providers.Logger;
using System;
using System.Net.Mail;
using System.Runtime.InteropServices;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using BP.Global.Delegates;
using System.Collections.Generic;

namespace BP.Service.Services.Core
{
    public class SMTPService : IDisposable
    {
        SmtpWorker _worker;
        SendGridClient _client;
        SmtpClient _smtp;
        CoreLoggerProvider _provider;

        public SMTPService()
        {
        }
        public SendGridClient Client
        {
            get
            {
                var sendgridAPI = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
                return _client ?? new SendGridClient(sendgridAPI);
            }
            private set
            {
                _client = value;
            }
        }
        public SmtpWorker Worker
        {
            get
            {
                return _worker ?? new SmtpWorker();
            }
            private set
            {
                _worker = value;
            }
        }
        public CoreLoggerProvider Provider
        {
            get
            {
                return _provider ?? new CoreLoggerProvider();
            }
            private set
            {
                _provider = value;
            }
        }
        public async Task SendGmailMessage(MailMessage message)
        {
            await _smtp.SendMailAsync(message);
        }
        bool mailSent = false;
        private async Task SendCompletedCallback(string purpose, bool success, IEnumerable<string> tos)
        {
            var subject = "Sending an email";
            var system = "Core System";
            var instigator = "SMTPService";
            var message = success ?
                $"Email was sent successfully for {purpose}" :
                $"Could not send an email for {purpose} due to an error";
            foreach(var to in tos)
            {
                instigator = to;
                await Provider.CreateNewLog(message, subject, instigator, system);
            }
            mailSent = true;
        }

        public async Task SendMailAsync(string purpose, SendGridMessage message, IEnumerable<string> tos)
        {
            var response = await Client.SendEmailAsync(message);
            await SendCompletedCallback(purpose, response.StatusCode == System.Net.HttpStatusCode.Accepted, tos);
            //_worker.EmailSend += SendCompletedCallback;
            //_worker.SendEmail(response.IsCompleted, purpose);
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
                _worker.Dispose();
                _provider.Dispose();
            }
            disposed = true;
        }

    }
}
