using Microsoft.Win32.SafeHandles;
using BP.Global.Models.Logger;
using BP.Logger.Database;
using System;
using System.Linq;
using System.Data.Entity;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace BP.Service.Services.Logger
{
    public class CoreLoggerServices : IDisposable
    {
        // Flag: Has Dispose already been called?
        bool disposed = false;
        // Instantiate a SafeHandle instance.
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

        public async Task<TEntity> FindObject<TEntity>(string name, BPLoggerContext context) where TEntity : class, INameId
        {
            if (context.Set<TEntity>().Any(y => y.Name.ToLower().Contains(name.ToLower())))
                return await context.Set<TEntity>().FirstOrDefaultAsync(y => y.Name.ToLower().Contains(name.ToLower()));
            else
            {
                var entity = Activator.CreateInstance<TEntity>();
                entity.Name = name;
                return entity;
            }
        }

        public async Task CreateLogAsync(Log log, BPLoggerContext context)
        {
            // this is going to create the log in the system
            context.Logs.Add(log);
            await context.SaveChangesAsync();
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
