namespace MyAbilityFirst.Infrastructure.Data.Migrations
{
	using System;
	using System.Data.Entity;
	using System.Data.Entity.Migrations;
	using System.Linq;

	internal sealed class Configuration : DbMigrationsConfiguration<MyAbilityFirst.Infrastructure.Data.MyAbilityFirstDbContext>
	{
		public Configuration()
		{
			AutomaticMigrationsEnabled = false;
		}

		protected override void Seed(MyAbilityFirst.Infrastructure.Data.MyAbilityFirstDbContext context)
		{
			// do nothing
			// manually run /Sql/Seed.sql to seed data from scratch
		}
	}
}
