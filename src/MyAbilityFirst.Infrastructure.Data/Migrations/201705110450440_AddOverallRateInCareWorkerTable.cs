namespace MyAbilityFirst.Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOverallRateInCareWorkerTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CareWorker", "TotalRating", c => c.Double(nullable: false));
            AddColumn("dbo.CareWorker", "RatingCount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CareWorker", "RatingCount");
            DropColumn("dbo.CareWorker", "TotalRating");
        }
    }
}
