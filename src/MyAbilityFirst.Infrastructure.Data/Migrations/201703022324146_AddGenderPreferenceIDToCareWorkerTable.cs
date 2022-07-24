namespace MyAbilityFirst.Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGenderPreferenceIDToCareWorkerTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CareWorker", "GenderPreferenceID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CareWorker", "GenderPreferenceID");
        }
    }
}
