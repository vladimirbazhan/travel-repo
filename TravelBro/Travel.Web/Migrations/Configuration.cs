using System.Diagnostics;
using WebApplication1.Models.EntityModels;

namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<WebApplication1.Models.IdentityModels.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "WebApplication1.Models.IdentityModels.ApplicationDbContext";
        }

        protected override void Seed(WebApplication1.Models.IdentityModels.ApplicationDbContext context)
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

            // fill TransType table with hardcoded data
            foreach (TransTypeEnum transpType in Enum.GetValues(typeof (TransTypeEnum)))
            {
                context.TransTypes.AddOrUpdate(x => x.Id, new TransType() {Id = (int)transpType, Name = transpType.ToString()});
            }
            context.SaveChanges();
        }
    }
}
