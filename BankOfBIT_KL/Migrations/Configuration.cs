namespace BankOfBIT_KL.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BankOfBIT_KL.Data.BankOfBIT_KLContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "BankOfBIT_KL.Data.BankOfBIT_KLContext";
        }

        protected override void Seed(BankOfBIT_KL.Data.BankOfBIT_KLContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}
