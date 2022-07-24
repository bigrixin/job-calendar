namespace MyAbilityFirst.Infrastructure.Data.Migrations
{
	using System;
	using System.Data.Entity.Migrations;

	public partial class AddOrganisationIdInCareWorkerTable : DbMigration
	{
		public override void Up()
		{
			AddColumn("dbo.CareWorker", "OrganisationId", c => c.Int(nullable: false));
			AddColumn("dbo.CareWorker", "StartingDate", c => c.DateTime());
		}

		public override void Down()
		{
			DropColumn("dbo.CareWorker", "StartingDate");
			DropColumn("dbo.CareWorker", "OrganisationId");
		}
	}
}

