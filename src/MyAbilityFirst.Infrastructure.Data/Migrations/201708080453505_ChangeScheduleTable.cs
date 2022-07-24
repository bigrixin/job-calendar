namespace MyAbilityFirst.Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeScheduleTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Schedule",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        JobID = c.Int(nullable: false),
                        Duration_Start = c.DateTime(nullable: false),
                        Duration_End = c.DateTime(nullable: false),
                        ScheduleType = c.Int(nullable: false),
                        EffectiveEndDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Booking", "Schedule_ID", c => c.Int());
            AlterColumn("dbo.Job", "JobStatus", c => c.Int(nullable: false));
            CreateIndex("dbo.Booking", "Schedule_ID");
            AddForeignKey("dbo.Booking", "Schedule_ID", "dbo.Schedule", "ID");
            DropColumn("dbo.Booking", "Schedule_Start");
            DropColumn("dbo.Booking", "Schedule_End");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Booking", "Schedule_End", c => c.DateTime(nullable: false));
            AddColumn("dbo.Booking", "Schedule_Start", c => c.DateTime(nullable: false));
            DropForeignKey("dbo.Booking", "Schedule_ID", "dbo.Schedule");
            DropIndex("dbo.Booking", new[] { "Schedule_ID" });
            AlterColumn("dbo.Job", "JobStatus", c => c.String());
            DropColumn("dbo.Booking", "Schedule_ID");
            DropTable("dbo.Schedule");
        }
    }
}
