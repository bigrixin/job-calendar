namespace MyAbilityFirst.Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAvailabilityTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Availability",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DayOfWeek = c.Int(nullable: false),
                        TimeOfDay = c.Int(nullable: false),
                        CareWorker_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CareWorker", t => t.CareWorker_ID)
                .Index(t => t.CareWorker_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Availability", "CareWorker_ID", "dbo.CareWorker");
            DropIndex("dbo.Availability", new[] { "CareWorker_ID" });
            DropTable("dbo.Availability");
        }
    }
}
