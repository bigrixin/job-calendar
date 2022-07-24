namespace MyAbilityFirst.Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRatingStateInRatingTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rating", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.Rating", "CoordinatorID", c => c.Int());
            AddColumn("dbo.Rating", "ApprovedDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Rating", "ApprovedDate");
            DropColumn("dbo.Rating", "CoordinatorID");
            DropColumn("dbo.Rating", "Status");
        }
    }
}
