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

            context.SaveChanges();
        }
    }
}
