using BP.Global.Models.Main;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Main.DataBase
{
    public partial class BPMainContext : DbContext
    {
        public BPMainContext() : base("name=BPMainContext") { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<PreferenceAttribute>()
                .HasMany(c => c.NewsLetters)
                .WithMany(s => s.PreferenceAttributes)
                .Map(cs => {
                    cs.MapLeftKey("NewsLettersPreferenceIds");
                    cs.MapRightKey("PreferenceNewsLettersIds");
                    cs.ToTable("PreferenceNewsLettersTable");
                });
        }

        #region Put DbSets Here
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<LoginAttribute> LoginAttributes { get; set; }
        public DbSet<LoginId> LoginIds { get; set; }
        public DbSet<LoginIdType> LoginIdTypes { get; set; }
        public DbSet<NewsLetter> NewsLetters { get; set; }
        public DbSet<PreferenceAttribute> PreferenceAttributes { get; set; }
        public DbSet<ProfileAttribute> ProfileAttributes { get; set; }
        #endregion


        public async Task<MethodResults> SaveChangesAsync(BPMainContext context)
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

        public MethodResults SaveChanges(BPMainContext context)
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

    public partial class BPMainContext
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
