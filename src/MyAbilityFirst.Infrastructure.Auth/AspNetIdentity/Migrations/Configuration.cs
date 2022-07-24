namespace MyAbilityFirst.Infrastructure.Auth.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MyAbilityFirst.Infrastructure.Auth.EntityFramework.AspNetIdentityDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "MyAbilityFirst.Infrastructure.Auth.EntityFramework.AspNetIdentityDbContext";
        }

        protected override void Seed(MyAbilityFirst.Infrastructure.Auth.EntityFramework.AspNetIdentityDbContext context)
        {
        }
    }
}
