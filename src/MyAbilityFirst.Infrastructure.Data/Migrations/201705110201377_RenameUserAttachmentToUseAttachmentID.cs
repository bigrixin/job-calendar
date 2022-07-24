namespace MyAbilityFirst.Infrastructure.Data.Migrations
{
	using System;
	using System.Data.Entity.Migrations;

	public partial class RenameUserAttachmentToUseAttachmentID : DbMigration
	{
		public override void Up()
		{
			RenameColumn(table: "dbo.UserAttachment", name: "SubcategoryID", newName: "AttachmentTypeID");
		}

		public override void Down()
		{
			RenameColumn(table: "dbo.UserAttachment", name: "AttachmentTypeID", newName: "SubcategoryID");
		}
	}
}
