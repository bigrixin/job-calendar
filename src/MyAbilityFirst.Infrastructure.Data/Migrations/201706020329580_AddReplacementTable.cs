namespace MyAbilityFirst.Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddReplacementTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Replacement",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        BookingID = c.Int(nullable: false),
                        CareWorkerID = c.Int(nullable: false),
                        CommentByCareWorker = c.String(),
                        ReplacedCareWorkerID = c.Int(),
												ReplacedBookingID=c.Int(),
                        CoordinatorID = c.Int(),
                        CommentByCoordinator = c.String(),
                        CreatedAt = c.DateTime(),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Replacement");
        }
    }
}
