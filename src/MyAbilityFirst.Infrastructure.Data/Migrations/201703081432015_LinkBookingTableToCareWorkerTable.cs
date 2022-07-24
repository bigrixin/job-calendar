namespace MyAbilityFirst.Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LinkBookingTableToCareWorkerTable : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Booking", "CareWorkerID");
            AddForeignKey("dbo.Booking", "CareWorkerID", "dbo.CareWorker", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Booking", "CareWorkerID", "dbo.CareWorker");
            DropIndex("dbo.Booking", new[] { "CareWorkerID" });
        }
    }
}
