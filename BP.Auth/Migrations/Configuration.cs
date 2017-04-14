namespace BP.Accounts.Migrations
{
    using BP.Auth;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BP.Auth.BPAuthContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(BPAuthContext context)
        {
            var cookieTypes = new List<CookieType> {
                new CookieType { Name = "Login", Index = 100 },
            };

            cookieTypes.ForEach(h => context.CookieTypes.AddOrUpdate(x => x.Index, h));

            var nameTypes = new List<NameType> {
                new NameType { Type = "First", Index = 100  },
                new NameType { Type = "Middle", Index = 110  },
                new NameType { Type = "Last", Index = 120  },
                new NameType { Type = "Nickname", Index = 130  },
                new NameType { Type = "Username", Index = 1000 },
                new NameType { Type = "Preferred name", Index = 2000 },
                new NameType { Type = "Alternate name", Index = 3000 },
                new NameType { Type = "Maiden name", Index = 4000 },
                new NameType { Type = "Secret name", Index = 5000 },
            };

            nameTypes.ForEach(h => context.NameTypes.AddOrUpdate(x => x.Index, h));

            var titles = new List<Title> {
                new Title { Name = "Dr", Index = 10 },
                new Title { Name = "Mr", Index = 20 },
                new Title { Name = "Mrs", Index = 30 },
                new Title { Name = "Ms", Index = 40 },
                new Title { Name = "Miss", Index = 50 },
                new Title { Name = "Professor", Index = 60 },
            };

            titles.ForEach(h => context.Titles.AddOrUpdate(x => x.Index, h));

            var phoneNumberTypes = new List<PhoneNumberType> {
                new PhoneNumberType { TypeName = "Home", Index = 10 },
                new PhoneNumberType { TypeName = "Home Fax", Index = 20 },
                new PhoneNumberType { TypeName = "Work", Index = 30 },
                new PhoneNumberType { TypeName = "Work Fax", Index = 40 },
                new PhoneNumberType { TypeName = "Mobile", Index = 50 },
                new PhoneNumberType { TypeName = "Work Mobile", Index = 60 },
                new PhoneNumberType { TypeName = "Spouse", Index = 70 },
                new PhoneNumberType { TypeName = "Spouse Mobile", Index = 80 },
            };

            phoneNumberTypes.ForEach(h => context.PhoneNumberTypes.AddOrUpdate(x => x.Index, h));

            var profileDateTypes = new List<ProfileDateType> {
                new ProfileDateType { TypeName = "Created", Index = 10 },
                new ProfileDateType { TypeName = "SetPassword", Index = 11 },
                new ProfileDateType { TypeName = "Viewed", Index = 20 },
                new ProfileDateType { TypeName = "Modified", Index = 30 },
                new ProfileDateType { TypeName = "UpdatedPassword", Index = 31 },
                new ProfileDateType { TypeName = "Removed", Index = 40 },
                new ProfileDateType { TypeName = "AddedTo", Index = 50 },
                new ProfileDateType { TypeName = "TakenAway", Index = 60 },
                new ProfileDateType { TypeName = "Indexed", Index = 70 },
                new ProfileDateType { TypeName = "Skipped", Index = 80 },
                new ProfileDateType { TypeName = "Searched", Index = 90 },
                new ProfileDateType { TypeName = "Voided", Index = 100 },
                new ProfileDateType { TypeName = "Locked", Index = 110 },
                new ProfileDateType { TypeName = "Verified", Index = 120 },
                new ProfileDateType { TypeName = "Failed", Index = 200 },
                new ProfileDateType { TypeName = "LoggedIn", Index = 300 },
                new ProfileDateType { TypeName = "LoggedOut", Index = 301 },
            };

            profileDateTypes.ForEach(h => context.ProfileDateTypes.AddOrUpdate(x => x.Index, h));

            var registrationTypes = new List<RegistrationType> {
                new RegistrationType { Name = "Account Only", Index = 100 },
                new RegistrationType { Name = "Visitor", Index = 200 },
                new RegistrationType { Name = "Standard", Index = 300 },
                new RegistrationType { Name = "Premium", Index = 400 },
                new RegistrationType { Name = "Business", Index = 500 },
                new RegistrationType { Name = "Internal", Index = 600 },
                new RegistrationType { Name = "Employee", Index = 700 },
            };

            registrationTypes.ForEach(h => context.RegistrationTypes.AddOrUpdate(x => x.Index, h));

            var claimEntities = new List<ClaimEntity> {
                new ClaimEntity { ClaimName = "Website", Index = 10 },
                new ClaimEntity { ClaimName = "API", Index = 20 },
                new ClaimEntity { ClaimName = "Social Media", Index = 30 },
                new ClaimEntity { ClaimName = "Mobile", Index = 40 },
                new ClaimEntity { ClaimName = "Internal", Index = 50 },
                new ClaimEntity { ClaimName = "Other", Index = 60 },
            };

            claimEntities.ForEach(h => context.ClaimEntities.AddOrUpdate(x => x.Index, h));

            var tokenProviders = new List<TokenProvider> {
                new TokenProvider { Provider = "Cookie", Index = 10 },
                new TokenProvider { Provider = "Username-password", Index = 20 },
                new TokenProvider { Provider = "Facebook", Index = 30 },
                new TokenProvider { Provider = "Google", Index = 40 },
                new TokenProvider { Provider = "Microsoft", Index = 50 },
                new TokenProvider { Provider = "Twitter", Index = 60 },
                new TokenProvider { Provider = "Github", Index = 70 },
                new TokenProvider { Provider = "BioMetrics", Index = 80 },
                new TokenProvider { Provider = "Pin Number", Index = 90 },
                new TokenProvider { Provider = "Certificate", Index = 100 },
                new TokenProvider { Provider = "Other", Index = 110 },
            };

            tokenProviders.ForEach(h => context.TokenProviders.AddOrUpdate(x => x.Index, h));

            var roleGroups = new List<RoleGroup> {
                new RoleGroup { Name = "External", Index = 100 },
                new RoleGroup { Name = "Anonymous", Index = 200 },
                new RoleGroup { Name = "Registered", Index = 300 },
                new RoleGroup { Name = "Business", Index = 400 },
                new RoleGroup { Name = "Company", Index = 500 },
                new RoleGroup { Name = "IT", Index = 1000 },
            };

            roleGroups.ForEach(h => context.RoleGroups.AddOrUpdate(x => x.Index, h));

            context.SaveChanges();

            var grp1 = context.RoleGroups.FirstOrDefault(x => x.Index == 100);
            var grp2 = context.RoleGroups.FirstOrDefault(x => x.Index == 200);
            var grp3 = context.RoleGroups.FirstOrDefault(x => x.Index == 300);
            var grp4 = context.RoleGroups.FirstOrDefault(x => x.Index == 400);
            var grp5 = context.RoleGroups.FirstOrDefault(x => x.Index == 500);
            var grp6 = context.RoleGroups.FirstOrDefault(x => x.Index == 1000);

            var roles = new List<Role> {
                new Role { Name = "External", RoleGroup = grp1, Index = 100 },
                new Role { Name = "Anonymous", RoleGroup = grp2, Index = 200 },
                new Role { Name = "Customer", RoleGroup = grp3, Index = 300 },
                new Role { Name = "Premium", RoleGroup = grp3, Index = 400 },
                new Role { Name = "Enterprise", RoleGroup = grp4, Index = 500 },
                new Role { Name = "Vendor", RoleGroup = grp4, Index = 600 },
                new Role { Name = "Partner", RoleGroup = grp4, Index = 700 },
                new Role { Name = "Moderator", RoleGroup = grp5, Index = 800 },
                new Role { Name = "Employee", RoleGroup = grp5, Index = 900 },
                new Role { Name = "Management", RoleGroup = grp5, Index = 1000 },
                new Role { Name = "WebMaster", RoleGroup = grp6, Index = 1100 },
            };

            roles.ForEach(h => context.Roles.AddOrUpdate(x => x.Index, h));

            context.SaveChanges();
        }
    }
}
