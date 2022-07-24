namespace MyAbilityFirst.Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStatusToUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "Status", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "Status");
        }
    }
}
