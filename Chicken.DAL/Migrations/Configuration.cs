namespace Chicken.DAL.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ChickenDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "Chicken.DAL.ChickenDbContext";
        }

        protected override void Seed(ChickenDbContext context)
        {
        }
    }
}
