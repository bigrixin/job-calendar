namespace MyAbilityFirst.Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNoticeFromToNotificationTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notification", "NoticeFrom", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Notification", "NoticeFrom");
        }
    }
}
