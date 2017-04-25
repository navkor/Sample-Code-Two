using BP.Global.Models.Logger;
using BPSystem = BP.Global.Models.Logger.System;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Logger.Database
{
    public partial class BPLoggerContext : DbContext
    {
        public BPLoggerContext() : base("name=BPLoggerContext")
        { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<SMTPMessage>()
                .HasMany(c => c.ToAddresses)
                .WithMany(s => s.SMTPMessages)
                .Map(cs => {
                    cs.MapLeftKey("ToSMTPMessageId");
                    cs.MapRightKey("SMTPMessageToId");
                    cs.ToTable("SMTPMessageToAddressTable");
                });
        }

        #region Put DbSets here
        public DbSet<Instigator> Instigators { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<BPSystem> Systems { get; set; }
        public DbSet<SMTPAccount> SMTPAccounts { get; set; }
        public DbSet<SMTPAttachment> SMTPAttachments { get; set; }
        public DbSet<SMTPMessage> SMTPMessages { get; set; }
        public DbSet<SMTPTo> SMTPTos { get; set; }
        public DbSet<SMTPSendDate> SMTPSendDates { get; set; }
        #endregion

        public async Task<MethodResults> SaveChangesAsync(BPLoggerContext context)
        {
            var methodResults = new MethodResults();
            try
            {
                await context.SaveChangesAsync();
                methodResults.Success = true;
                methodResults.Message = "Changes were saved successfully!";
            }
            catch (DbUpdateException ex)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var entry in ex.Entries)
                {
                    sb.AppendLine(entry.GetType().ToString());
                }
                methodResults.Message = StaticClasses.SystemDebug ? $"The list of problems with the update are: {sb.ToString()}" :
                    "The changes could not be made.  Please report this problem to your system administrator.";
            }
            return methodResults;
        }

        public MethodResults SaveChanges(BPLoggerContext context)
        {
            var methodResults = new MethodResults();
            try
            {
                context.SaveChanges();
                methodResults.Success = true;
                methodResults.Message = "Changes were saved successfully!";
            }
            catch (DbUpdateException ex)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var entry in ex.Entries)
                {
                    sb.AppendLine(entry.GetType().ToString());
                }
                methodResults.Message = StaticClasses.SystemDebug ? $"The list of problems with the update are: {sb.ToString()}" :
                    "The changes could not be made.  Please report this problem to your system administrator.";
            }
            return methodResults;
        }
    }

    public partial class BPLoggerContext
    {
        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                var errorMessage = ex.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage);

                var fullErrorMessage = string.Join("; ", errorMessage);

                var exceptionMessage = string.Concat(ex.Message, " the validation errors are: ", fullErrorMessage);

                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }
    }
}
