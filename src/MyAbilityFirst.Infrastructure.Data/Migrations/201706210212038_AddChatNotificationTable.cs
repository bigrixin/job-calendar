namespace MyAbilityFirst.Infrastructure.Data.Migrations
{
	using System.Data.Entity.Migrations;

	public partial class AddChatNotificationTable : DbMigration
	{
		public override void Up()
		{
			CreateTable(
					"dbo.ChatNotification",
					c => new
					{
						ID = c.Int(nullable: false),
						RoomID = c.Int(nullable: false),
						RedirectURL = c.String(),
						NewMessageCount = c.Int(nullable: false),
					})
					.PrimaryKey(t => t.ID)
					.ForeignKey("dbo.Notification", t => t.ID)
					.Index(t => t.ID);

		}

		public override void Down()
		{
			DropForeignKey("dbo.ChatNotification", "ID", "dbo.Notification");
			DropIndex("dbo.ChatNotification", new[] { "ID" });
			DropTable("dbo.ChatNotification");
		}
	}
}
