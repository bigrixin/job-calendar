namespace MyAbilityFirst.Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddClosedStatusToNotification : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notification", "Closed", c => c.Boolean(nullable: false));
            DropColumn("dbo.ChatNotification", "RedirectURL");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ChatNotification", "RedirectURL", c => c.String());
            DropColumn("dbo.Notification", "Closed");
        }
    }
}
