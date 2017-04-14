using Microsoft.Win32.SafeHandles;
using BP.Service.Providers.Logger;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
using System.Runtime.InteropServices;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Text;
using System.Threading.Tasks;
using BP.Global.Delegates;

namespace BP.Service.Services.Core
{    
    public class SMTPService : IDisposable
    {
        SendGridMessage _message;
        SmtpWorker _worker;
        SendGridClient _client;
        CoreLoggerProvider _provider;
        public SMTPService()
        {
            
        }
        public SMTPService(SendGridMessage message)
        {
            var sendgridAPI = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
            _client = new SendGridClient(sendgridAPI);
            _worker = new SmtpWorker();
            _message = message;
            _provider = new CoreLoggerProvider();
        }
        bool mailSent = false;
        private async Task SendCompletedCallback(string purpose, bool success)
        {
            var subject = "Sending an email";
            var system = "Core System";
            var instigator = "SMTPService";
            var message = success ?
                $"Email was sent successfully for {purpose}" :
                $"Could not send an email for {purpose} due to an error";
            await _provider.CreateNewLog(message, subject, instigator, system);
            mailSent = true;
        }

        public async Task SendMailAsync(string purpose)
        {
            var response = await _client.SendEmailAsync(_message);
            await SendCompletedCallback(purpose, response.StatusCode == System.Net.HttpStatusCode.Accepted);
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
