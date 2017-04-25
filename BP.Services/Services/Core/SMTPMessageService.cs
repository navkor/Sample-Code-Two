using BP.Global.Models.Logger;
using BP.Logger.Database;
using BP.VM.ViewModels.Core.SMTP;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BP.Services.Services.Core
{
    public class SMTPMessageService : IDisposable
    {

        public async Task<MethodResults> CreateMessage(SMTPCreateMessage message, BPLoggerContext context)
        {
            var methodResults = new MethodResults { Success = false, ID = 0, Message = "Something went wrong and the email could not be created." };

            // the first thing that needs to happen is the to addresses need to be scanned in the database to see if they've ever had emails sent to them before
            
            var smtpMessage = new SMTPMessage {
                Subject = message.Subject,
                Message = message.Message,
                SendDate = message.SendDate,
                Importance = message.Importance,
                ToAddresses = await toAddress(message.ToAddresses.ToList(), context)
            };

            var dateDifference = new TimeSpan();

            dateDifference = message.SendDate - DateTime.Now;

            // find out if the date is today or in the future

            if (dateDifference.Days == 0)
            {
                // they are sending today
                if (message.Importance == 1)
                {
                    // they need to send now

                }
            }

            return methodResults;
        }

        private async Task<ICollection<SMTPTo>> toAddress(IEnumerable<SMTPTOAddress> to, BPLoggerContext context)
        {
            var toBag = new ConcurrentBag<SMTPTo>();
            await Task.Run(() => Parallel.ForEach(to, toAddress => {
                var toAdd = context.SMTPTos.FirstOrDefault(x => x.EmailAddress.Equals(toAddress.EmailAddress));
                if (toAdd == null) {
                    toAdd = new SMTPTo
                    {
                        EmailAddress = toAddress.EmailAddress,
                        Name = toAddress.Name
                    };
                    context.SMTPTos.Add(toAdd);
                    context.SaveChangesAsync();
                    toBag.Add(toAdd);
                }
                toBag.Add(toAdd);
            }));
            return toBag.ToList();
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
            }
            disposed = true;
        }
    }
}
