namespace MyAbilityFirst.Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddReplacementsInBookingTable : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Replacement", "BookingID");
            AddForeignKey("dbo.Replacement", "BookingID", "dbo.Booking", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Replacement", "BookingID", "dbo.Booking");
            DropIndex("dbo.Replacement", new[] { "BookingID" });
        }
    }
}
