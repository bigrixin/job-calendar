namespace MyAbilityFirst.Infrastructure.Data.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddNotificationTable : DbMigration
    {
		public override void Up()
		{
			CreateTable(
					"dbo.Notification",
					c => new
					{
						ID = c.Int(nullable: false, identity: true),
						OwnerUserID = c.Int(nullable: false),
						NotifiedDate = c.DateTime(nullable: false),
						ReadDate = c.DateTime(),
					})
					.PrimaryKey(t => t.ID)
					.ForeignKey("dbo.User", t => t.OwnerUserID, cascadeDelete: true)
					.Index(t => t.OwnerUserID);

			RenameColumn("dbo.Client", "Notifications_ReceiveEmailNotifications", "NotificationSettings_ReceiveEmailNotifications");
			RenameColumn("dbo.Client", "Notifications_ReceiveSMSNotifications", "NotificationSettings_ReceiveSMSNotifications");

		}

		public override void Down()
		{
			RenameColumn("dbo.Client", "NotificationSettings_ReceiveSMSNotifications", "Notifications_ReceiveSMSNotifications");
			RenameColumn("dbo.Client", "NotificationSettings_ReceiveEmailNotifications", "Notifications_ReceiveEmailNotifications");

			DropForeignKey("dbo.Notification", "OwnerUserID", "dbo.User");
			DropIndex("dbo.Notification", new[] { "OwnerUserID" });
			DropTable("dbo.Notification");
		}
	}
}
