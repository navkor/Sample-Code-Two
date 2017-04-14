using Microsoft.Win32.SafeHandles;
using BP.Logger.Database;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Data.Entity;
using System.Threading.Tasks;
using BP.Service.Services.Logger;
using BP.Global.Models.Logger;

namespace BP.Service.Providers.Logger
{
    public class CoreLoggerProvider : IDisposable
    {
        BPLoggerContext _context;
        CoreLoggerServices _service;
        public CoreLoggerProvider()
        {
            _context = new BPLoggerContext();
            _service = new CoreLoggerServices();
        }
        // Flag: Has Dispose already been called?
        bool disposed = false;
        // Instantiate a SafeHandle instance.
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

        public async Task CreateNewLog(string body, string subject, string instigator, string system)
        {
            var subjectObject = _context.Subjects.Any(y => y.SubjectLine.ToLower().Contains(subject.ToLower())) ?
                await _context.Subjects.FirstOrDefaultAsync(y => y.SubjectLine.ToLower() == subject.ToLower())
                : new Subject
                {
                    SubjectLine = subject
                };
            var instigatorObject = await _service.FindObject<Instigator>(instigator, _context);
            var systemObject = await _service.FindObject<Global.Models.Logger.System>(system, _context);
            await _service.CreateLogAsync(new Log {
                Body = body,
                Subject = subjectObject,
                Instigator = instigatorObject,
                System = systemObject,
                DateLine = DateTimeOffset.Now
            }, _context);                
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
                _context.Dispose();
                _service.Dispose();
            }
            disposed = true;
        }
    }
}
