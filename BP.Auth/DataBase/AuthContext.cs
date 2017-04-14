using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Auth
{
    public partial class BPAuthContext : DbContext
    {
        public BPAuthContext() : base("name=BPAuthContext")
        { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        #region put DBSets here
        public DbSet<AccountAttribute> AccountAttributes { get; set; }
        public DbSet<ClaimEntity> ClaimEntities { get; set; }
        public DbSet<Cookie>  Cookies { get; set; }
        public DbSet<CookieType> CookieTypes { get; set; }
        public DbSet<DateTable> DateTables { get; set; }
        public DbSet<EmailAddress> EmailAddresses { get; set; }
        public DbSet<IdentityAttribute> IdentityAttributes { get; set; }
        public DbSet<LoginAttribute> LoginAttributes { get; set; }
        public DbSet<LoginToken> LoginTokens { get; set; }
        public DbSet<NameType> NameTypes { get; set; }
        public DbSet<PhoneNumber> PhoneNumbers { get; set; }
        public DbSet<PhoneNumberType> PhoneNumberTypes { get; set; }
        public DbSet<ProfileAttribute> ProfileAttributes { get; set; }
        public DbSet<ProfileDateType> ProfileDateTypes { get; set; }
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<RegistrationType> RegistrationTypes { get; set; }
        public DbSet<ResetQuestion> ResetQuestions { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleGroup> RoleGroups { get; set; }
        public DbSet<Title> Titles { get; set; }
        public DbSet<TokenClaim> TokenClaims { get; set; }
        public DbSet<TokenProvider> TokenProviders { get; set; }
        public DbSet<UserName> UserNames { get; set; }
        #endregion

        public async Task<MethodResults> SaveChangesAsync(BPAuthContext context)
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

        public MethodResults SaveChanges(BPAuthContext context)
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

    public partial class BPAuthContext
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
