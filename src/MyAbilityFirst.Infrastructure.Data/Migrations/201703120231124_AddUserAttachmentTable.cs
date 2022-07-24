namespace MyAbilityFirst.Infrastructure.Data.Migrations
{
	using System;
	using System.Data.Entity.Migrations;

	public partial class AddUserAttachmentTable : DbMigration
	{
		public override void Up()
		{
			CreateTable(
					"dbo.UserAttachment",
					c => new
					{
						ID = c.Int(nullable: false, identity: true),
						UserID = c.Int(nullable: false),
						URL = c.String(),
						SubcategoryID = c.Int(nullable: false),
					})
					.PrimaryKey(t => t.ID)
					.ForeignKey("dbo.User", t => t.UserID)
					.Index(t => t.UserID);
		}

		public override void Down()
		{
			DropForeignKey("dbo.UserAttachment", "UserID", "dbo.User");
			DropIndex("dbo.Job", new[] { "UserID" });
			DropTable("dbo.UserAttachment");
		}
	}
}
