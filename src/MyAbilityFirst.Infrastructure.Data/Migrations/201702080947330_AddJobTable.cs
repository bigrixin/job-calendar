namespace MyAbilityFirst.Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddJobTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Job",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        SuburbId = c.Int(nullable: false),
                        GenderId = c.Int(nullable: false),
                        ServiceId = c.Int(nullable: false),
                        ServiceAt = c.DateTime(),
                        JobStatus = c.String(),
                        ClientId = c.Int(nullable: false),
                        CreatedAt = c.DateTime(),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Client", t => t.ClientId)
                .Index(t => t.ClientId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Job", "ClientId", "dbo.Client");
            DropIndex("dbo.Job", new[] { "ClientId" });
            DropTable("dbo.Job");
        }
    }
}
