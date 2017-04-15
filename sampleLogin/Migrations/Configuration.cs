namespace sampleLogin.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using sampleLogin.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<sampleLogin.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(sampleLogin.Models.ApplicationDbContext context)
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
            var roleGroups = new List<RoleGroup> {
                new RoleGroup { Name = "Customer", Index = 100 },
                new RoleGroup { Name = "Premium", Index = 200 },
                new RoleGroup { Name = "Business", Index = 300 },
                new RoleGroup { Name = "Vendor", Index = 400 },
                new RoleGroup { Name = "Partner", Index = 500 },
                new RoleGroup { Name = "Moderator", Index = 600 },
                new RoleGroup { Name = "Employee", Index = 700 },
                new RoleGroup { Name = "IT", Index = 800 },
                new RoleGroup { Name = "WebMaster", Index = 1000 },
            };

            var roles = new List<ApplicationRole> {
                new ApplicationRole { Name = "User", Index = 1, RoleGroup = roleGroups.FirstOrDefault(x => x.Index == 100) },
                new ApplicationRole { Name = "Advanced User", Index = 100, RoleGroup = roleGroups.FirstOrDefault(x => x.Index == 100) },
                new ApplicationRole { Name = "Premium User", Index = 1000, RoleGroup = roleGroups.FirstOrDefault(x => x.Index == 200) },
                new ApplicationRole { Name = "Business", Index = 2000, RoleGroup = roleGroups.FirstOrDefault(x => x.Index == 300) },
                new ApplicationRole { Name = "Premium Business", Index = 2500, RoleGroup = roleGroups.FirstOrDefault(x => x.Index == 300) },
                new ApplicationRole { Name = "Vendor", Index = 3000, RoleGroup = roleGroups.FirstOrDefault(x => x.Index == 400) },
                new ApplicationRole { Name = "Premium Vendor", Index = 3500, RoleGroup = roleGroups.FirstOrDefault(x => x.Index == 400) },
                new ApplicationRole { Name = "Partner", Index = 4000, RoleGroup = roleGroups.FirstOrDefault(x => x.Index == 500) },
                new ApplicationRole { Name = "Moderator", Index = 4500, RoleGroup = roleGroups.FirstOrDefault(x => x.Index == 600) },
                new ApplicationRole { Name = "Moderator Supervisor", Index = 4900, RoleGroup = roleGroups.FirstOrDefault(x => x.Index == 600) },
                new ApplicationRole { Name = "Employee", Index = 5000, RoleGroup = roleGroups.FirstOrDefault(x => x.Index == 700) },
                new ApplicationRole { Name = "Supervisor", Index = 5200, RoleGroup = roleGroups.FirstOrDefault(x => x.Index == 700) },
                new ApplicationRole { Name = "Manager", Index = 5500, RoleGroup = roleGroups.FirstOrDefault(x => x.Index == 700) },
                new ApplicationRole { Name = "Director", Index = 5900, RoleGroup = roleGroups.FirstOrDefault(x => x.Index == 700) },
                new ApplicationRole { Name = "IT", Index = 6000, RoleGroup = roleGroups.FirstOrDefault(x => x.Index == 800) },
                new ApplicationRole { Name = "IT Supervisor", Index = 6200, RoleGroup = roleGroups.FirstOrDefault(x => x.Index == 800) },
                new ApplicationRole { Name = "IT Manager", Index = 6500, RoleGroup = roleGroups.FirstOrDefault(x => x.Index == 800) },
                new ApplicationRole { Name = "IT Director", Index = 6900, RoleGroup = roleGroups.FirstOrDefault(x => x.Index == 800) },
                new ApplicationRole { Name = "Webmaster", Index = 10000, RoleGroup = roleGroups.FirstOrDefault(x => x.Index == 1000) },
                new ApplicationRole { Name = "Webmaster Manager", Index = 11111, RoleGroup = roleGroups.FirstOrDefault(x => x.Index == 1000) },
            };

            foreach (var identity in roles)
            {
                if (!context.Roles.Any(y => y.Name == identity.Name))
                {
                    var store = new RoleStore<ApplicationRole>(context);
                    var manager = new RoleManager<ApplicationRole>(store);
                    manager.Create(identity);
                }
            }
        }
    }
}
