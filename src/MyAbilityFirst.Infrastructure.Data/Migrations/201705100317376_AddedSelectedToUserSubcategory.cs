namespace MyAbilityFirst.Infrastructure.Data.Migrations
{
	using System;
	using System.Data.Entity.Migrations;

	public partial class AddedSelectedToUserSubcategory : DbMigration
	{
		public override void Up()
		{
			RenameColumn(table: "dbo.UserSubcategory", name: "UserID", newName: "OwnerUserID");
			AddColumn("dbo.UserSubcategory", "Selected", c => c.Boolean(nullable: false));
			CreateIndex("dbo.UserSubcategory", "OwnerUserID");
			AddForeignKey("dbo.UserSubcategory", "OwnerUserID", "dbo.User", "ID", cascadeDelete: true);
		}

		public override void Down()
		{
			DropForeignKey("dbo.UserSubcategory", "OwnerUserID", "dbo.User");
			DropIndex("dbo.UserSubcategory", new[] { "OwnerUserID" });
			DropColumn("dbo.UserSubcategory", "Selected");
			RenameColumn(table: "dbo.UserSubcategory", name: "OwnerUserID", newName: "UserID");
		}
	}
}
