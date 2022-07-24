namespace MyAbilityFirst.Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenamedUserIDToOwnerUserID : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Shortlist", name: "UserID", newName: "OwnerUserID");
            RenameIndex(table: "dbo.Shortlist", name: "IX_UserID", newName: "IX_OwnerUserID");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Shortlist", name: "IX_OwnerUserID", newName: "IX_UserID");
            RenameColumn(table: "dbo.Shortlist", name: "OwnerUserID", newName: "UserID");
        }
    }
}
