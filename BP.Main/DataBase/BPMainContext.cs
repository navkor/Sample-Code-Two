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

            modelBuilder.Entity<LoginPreferenceAttribute>()
                .HasMany(c => c.NewsLetters)
                .WithMany(s => s.PreferenceAttributes)
                .Map(cs => {
                    cs.MapLeftKey("NewsLettersPreferenceIds");
                    cs.MapRightKey("PreferenceNewsLettersIds");
                    cs.ToTable("PreferenceNewsLettersTable");
                });
            modelBuilder.Entity<EntityPreferenceAttribute>()
                .HasMany(c => c.AvoidAccounts)
                .WithMany(s => s.AvoidEntities)
                .Map(cs => {
                    cs.MapLeftKey("EntityAccountId");
                    cs.MapRightKey("AccountEntityId");
                    cs.ToTable("AvoidAccountPreferenceTable");
                });
            modelBuilder.Entity<EntityPreferenceAttribute>()
                .HasMany(c => c.PreferredAccounts)
                .WithMany(s => s.PreferredEntities)
                .Map(cs => {
                    cs.MapLeftKey("PreferredAccountId");
                    cs.MapRightKey("AccountEntityId");
                    cs.ToTable("PreferredAccountPreferenceTable");
                });
            modelBuilder.Entity<EntityPreferenceAttribute>()
                .HasMany(c => c.AvoidKeywords)
                .WithMany(s => s.AvoidEntities)
                .Map(cs => {
                    cs.MapLeftKey("EntityKeyWordId");
                    cs.MapRightKey("KeyWordEntityId");
                    cs.ToTable("AvoidKeyWordPreferenceTable");
                });
            modelBuilder.Entity<EntityPreferenceAttribute>()
                .HasMany(c => c.PreferredKeywords)
                .WithMany(s => s.PreferredEntities)
                .Map(cs => {
                    cs.MapLeftKey("EntityKeywordId");
                    cs.MapRightKey("KeyWordEntityId");
                    cs.ToTable("PreferredKeyWordPreferenceTable");
                });
        }

        #region Put DbSets Here
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountDate> AccountDates { get; set; }
        public DbSet<AccountDateType> AccountDateTypes { get; set; }
        public DbSet<AccountType> AccountTypes { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<AddressCityAttribute> AddressCityAttributes { get; set; }
        public DbSet<AddressType> AddressTypes { get; set; }
        public DbSet<BusinessAttribute> BusinessAttributes { get; set; }
        public DbSet<BusinessProgramAttribute> BusinessProgramAttributes { get; set; }
        public DbSet<BusinessType> BusinessTypes { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<EntityAttribute> EntityAttributes { get; set; }
        public DbSet<EntityPreferenceAttribute> EntityPreferenceAttributes { get; set; }
        public DbSet<FormName> FormNames { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<KeyWord> KeyWords { get; set; }
        public DbSet<LoginAttribute> LoginAttributes { get; set; }
        public DbSet<LoginId> LoginIds { get; set; }
        public DbSet<LoginIdType> LoginIdTypes { get; set; }
        public DbSet<LoginPreferenceAttribute> LoginPreferenceAttributes { get; set; }
        public DbSet<NameAttribute> NameAttributes { get; set; }
        public DbSet<NameFormat> NameFormats { get; set; }
        public DbSet<NameFormatMap> NameFormatMaps { get; set; }
        public DbSet<NewsLetter> NewsLetters { get; set; }
        public DbSet<PostalCode> PostalCodes { get; set; }
        public DbSet<ProfileAttribute> ProfileAttributes { get; set; }
        public DbSet<StreetAttribute> StreetAttributes { get; set; }
        public DbSet<StringTable> StringTables { get; set; }
        public DbSet<Title> Titles { get; set; }
        public DbSet<UserAttribute> UserAttributes { get; set; }
        public DbSet<UserName> UserNames { get; set; }
        public DbSet<UserNameType> UserNameTypes { get; set; }
        public DbSet<UserProgramAttribute> UserProgramAttributes { get; set; }
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
