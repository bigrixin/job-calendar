namespace MyAbilityFirst.Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MoveShortlistToUserTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Shortlist", "ClientID", "dbo.Client");
            RenameColumn(table: "dbo.Shortlist", name: "ClientID", newName: "UserID");
            RenameIndex(table: "dbo.Shortlist", name: "IX_ClientID", newName: "IX_UserID");
            AddColumn("dbo.Shortlist", "SelectedUserID", c => c.Int(nullable: false));
            AddForeignKey("dbo.Shortlist", "UserID", "dbo.User", "ID", cascadeDelete: true);
            DropColumn("dbo.Shortlist", "CareWorkerID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Shortlist", "CareWorkerID", c => c.Int(nullable: false));
            DropForeignKey("dbo.Shortlist", "UserID", "dbo.User");
            DropColumn("dbo.Shortlist", "SelectedUserID");
            RenameIndex(table: "dbo.Shortlist", name: "IX_UserID", newName: "IX_ClientID");
            RenameColumn(table: "dbo.Shortlist", name: "UserID", newName: "ClientID");
            AddForeignKey("dbo.Shortlist", "ClientID", "dbo.Client", "ID");
        }
    }
}
