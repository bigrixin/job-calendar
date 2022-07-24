namespace MyAbilityFirst.Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumeInScheduleTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Schedule", "Interval", c => c.Int(nullable: false));
            AddColumn("dbo.Schedule", "Position", c => c.Int(nullable: false));
            AddColumn("dbo.Schedule", "ByDay", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Schedule", "ByDay");
            DropColumn("dbo.Schedule", "Position");
            DropColumn("dbo.Schedule", "Interval");
        }
    }
}
