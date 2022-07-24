namespace MyAbilityFirst.Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSelectedToAvailabilityTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Availability", "Selected", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Availability", "Selected");
        }
    }
}
