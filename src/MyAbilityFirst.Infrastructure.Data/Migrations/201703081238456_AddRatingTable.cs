namespace MyAbilityFirst.Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRatingTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Rating",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        BookingID = c.Int(nullable: false),
                        OverallScore = c.Int(nullable: false),
                        Comments = c.String(),
                        PunctualityScore = c.Int(),
                        CommunicationScore = c.Int(),
                        ServiceQualityScore = c.Int(),
                        SatisfactionCheckingScore = c.Int(),
                        ResponsivenessScore = c.Int(),
                        FollowUpScore = c.Int(),
                        FutureRecommendationScore = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Booking", t => t.BookingID, cascadeDelete: true)
                .Index(t => t.BookingID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Rating", "BookingID", "dbo.Booking");
            DropIndex("dbo.Rating", new[] { "BookingID" });
            DropTable("dbo.Rating");
        }
    }
}
