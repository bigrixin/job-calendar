namespace MyAbilityFirst.Infrastructure.Data.Migrations
{
	using System;
	using System.Data.Entity.Migrations;

	public partial class AddIndexInCareWorkerTable : DbMigration
	{
		public override void Up()
		{
			CreateIndex("dbo.CareWorker", "OrganisationId");
		}

		public override void Down()
		{
			DropIndex("dbo.CareWorker", new[] { "OrganisationId" });
		}
	}
}
