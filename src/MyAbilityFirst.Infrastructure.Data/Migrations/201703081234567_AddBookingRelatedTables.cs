namespace MyAbilityFirst.Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBookingRelatedTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Booking",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ClientID = c.Int(nullable: false),
                        CareWorkerID = c.Int(nullable: false),
                        JobID = c.Int(),
                        Schedule_Start = c.DateTime(nullable: false),
                        Schedule_End = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Client", t => t.ClientID)
                .Index(t => t.ClientID);
            
            CreateTable(
                "dbo.CaseNote",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        BookingID = c.Int(nullable: false),
                        OwnerUserID = c.Int(nullable: false),
                        Note = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Booking", t => t.BookingID, cascadeDelete: true)
                .Index(t => t.BookingID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Booking", "ClientID", "dbo.Client");
            DropForeignKey("dbo.CaseNote", "BookingID", "dbo.Booking");
            DropIndex("dbo.CaseNote", new[] { "BookingID" });
            DropIndex("dbo.Booking", new[] { "ClientID" });
            DropTable("dbo.CaseNote");
            DropTable("dbo.Booking");
        }
    }
}
