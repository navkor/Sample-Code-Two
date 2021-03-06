namespace BP.Main.Migrations
{
    using BP.Global.Models.Main;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DataBase.BPMainContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DataBase.BPMainContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            var loginTypes = new List<LoginIdType> {
                new LoginIdType { Index = 1, Name = "User" },
                new LoginIdType { Index = 2, Name = "Business" },
                new LoginIdType { Index = 3, Name = "Primary" },
                new LoginIdType { Index = 4, Name = "Owner" },
                new LoginIdType { Index = 5, Name = "Guest" }
            };

            loginTypes.ForEach(h => context.LoginIdTypes.AddOrUpdate(s => s.Index, h));

            var genders = new List<Gender> {
                new Gender { Index = 1, Name = "Male" },
                new Gender { Index = 2, Name = "Female" }
            };

            genders.ForEach(h => context.Genders.AddOrUpdate(s => s.Index, h));

            var userNameTypes = new List<UserNameType> {
                new UserNameType { Index = 1, Name = "First Name" },
                new UserNameType { Index = 2, Name = "Last Name" },
                new UserNameType { Index = 3, Name = "Surname" },
                new UserNameType { Index = 4, Name = "Nickname" },
                new UserNameType { Index = 5, Name = "Pseudoname" },
                new UserNameType { Index = 6, Name = "Maiden Name" },
                new UserNameType { Index = 7, Name = "Secret Name" },
                new UserNameType { Index = 8, Name = "Middle Name" },
            };

            userNameTypes.ForEach(h => context.UserNameTypes.AddOrUpdate(s => s.Index, h));


            var titles = new List<Title> {
                new Title { Index = 10, Name = "Dr" },
                new Title { Index = 20, Name = "Mr" },
                new Title { Index = 30, Name = "Mrs" },
                new Title { Index = 40, Name = "Mis" },
                new Title { Index = 50, Name = "Miss" },
                new Title { Index = 60, Name = "Jr" },
                new Title { Index = 70, Name = "Master" },
                new Title { Index = 80, Name = "King" },
                new Title { Index = 90, Name = "Queen" },
                new Title { Index = 100, Name = "Prince" },
                new Title { Index = 110, Name = "Princess" },
            };

            titles.ForEach(h => context.Titles.AddOrUpdate(s => s.Index, h));


            context.SaveChanges();

            var first = context.UserNameTypes.FirstOrDefault(x => x.Index == 1);
            var middle = context.UserNameTypes.FirstOrDefault(x => x.Index == 8);
            var last = context.UserNameTypes.FirstOrDefault(x => x.Index == 2);
            var sur = context.UserNameTypes.FirstOrDefault(x => x.Index == 3);

            var firstmiddlelast = new List<NameFormatMap> {
                new NameFormatMap { UserNameType = first, DisplayOrder = 1 },
                new NameFormatMap { UserNameType = middle, DisplayOrder = 2 },
                new NameFormatMap { UserNameType = last, DisplayOrder = 3 },
            };

            var firstMiddleSur = new List<NameFormatMap> {
                new NameFormatMap { UserNameType = first, DisplayOrder = 1 },
                new NameFormatMap { UserNameType = middle, DisplayOrder = 2 },
                new NameFormatMap { UserNameType = sur, DisplayOrder = 3 },
            };

            var nameFormats = new List<NameFormat> {
                new NameFormat { Name = "US Standard", Index = 10, NameFormatMaps = firstmiddlelast },
                new NameFormat { Name = "UK Standard", Index = 20, NameFormatMaps = firstMiddleSur }
            };

            nameFormats.ForEach(h => context.NameFormats.AddOrUpdate(s => s.Index, h));

            context.SaveChanges();

            var accountDateTypes = new List<AccountDateType> {
                new AccountDateType { Index = 1, Name = "Created" },
                new AccountDateType { Index = 2, Name = "Edited" },
                new AccountDateType { Index = 3, Name = "Approved" },
                new AccountDateType { Index = 4, Name = "Locked" },
                new AccountDateType { Index = 5, Name = "Blocked" },
                new AccountDateType { Index = 6, Name = "Removed" },
                new AccountDateType { Index = 7, Name = "Trial Started" },
                new AccountDateType { Index = 8, Name = "Trial Ending" },
                new AccountDateType { Index = 9, Name = "Trial Ended" },
            };

            accountDateTypes.ForEach(h => context.AccountDateTypes.AddOrUpdate(s => s.Index, h));

            var accountTypes = new List<AccountType> {
                new AccountType { Index = 1, Name = "User" },
                new AccountType { Index = 2, Name = "Business" },
                new AccountType { Index = 3, Name = "Trial User" },
                new AccountType { Index = 4, Name = "Trial Business" },
                new AccountType { Index = 5, Name = "Sample User" },
                new AccountType { Index = 6, Name = "Sample Business" },
                new AccountType { Index = 7, Name = "Test Account" },
            };

            accountTypes.ForEach(h => context.AccountTypes.AddOrUpdate(s => s.Index, h));

            context.SaveChanges();

            var businessTypes = new List<BusinessType> {
                new BusinessType { Index = 1, Name = "Retail" },
                new BusinessType { Index = 2, Name = "Wholesale" },
                new BusinessType { Index = 3, Name = "BusinessToBusiness" },
                new BusinessType { Index = 4, Name = "Service" },
                new BusinessType { Index = 5, Name = "Retail Service" },
                new BusinessType { Index = 6, Name = "Wholesale Service" },
                new BusinessType { Index = 7, Name = "Maintenance" },
                new BusinessType { Index = 8, Name = "Versatile" },
                new BusinessType { Index = 9, Name = "Government" },
                new BusinessType { Index = 10, Name = "Manufacturing" }
            };

            businessTypes.ForEach(h => context.BusinessTypes.AddOrUpdate(s => s.Index, h));

            var presidenceTypes = new List<PresidenceType> {
                new PresidenceType { Index = 1, Name = "Owner" },
                new PresidenceType { Index = 2, Name = "Primary" },
                new PresidenceType { Index = 3, Name = "Manager" },
                new PresidenceType { Index = 4, Name = "Employee" },
                new PresidenceType { Index = 5, Name = "Access Granted" },
                new PresidenceType { Index = 6, Name = "Guest" },
                new PresidenceType { Index = 7, Name = "Stranger" },
            };

            presidenceTypes.ForEach(h => context.PresidenceTypes.AddOrUpdate(s => s.Index, h));

            context.SaveChanges();
        }
    }
}
