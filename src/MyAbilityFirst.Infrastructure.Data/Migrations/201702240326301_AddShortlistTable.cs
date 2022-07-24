namespace MyAbilityFirst.Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddShortlistTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Shortlist",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ClientID = c.Int(nullable: false),
                        CareWorkerID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Client", t => t.ClientID)
                .Index(t => t.ClientID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Shortlist", "ClientID", "dbo.Client");
            DropIndex("dbo.Shortlist", new[] { "ClientID" });
            DropTable("dbo.Shortlist");
        }
    }
}
