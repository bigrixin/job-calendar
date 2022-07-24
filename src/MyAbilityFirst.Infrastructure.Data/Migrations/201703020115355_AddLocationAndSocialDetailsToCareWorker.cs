namespace MyAbilityFirst.Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLocationAndSocialDetailsToCareWorker : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CareWorker", "ProvisionLocationSuburbID", c => c.Int(nullable: false));
            AddColumn("dbo.CareWorker", "LocationCoverageRadius", c => c.Int(nullable: false));
            AddColumn("dbo.CareWorker", "FirstLanguageID", c => c.Int(nullable: false));
            AddColumn("dbo.CareWorker", "SecondLanguageID", c => c.Int(nullable: false));
            AddColumn("dbo.CareWorker", "CultureID", c => c.Int(nullable: false));
            AddColumn("dbo.CareWorker", "ReligionID", c => c.Int(nullable: false));
            AddColumn("dbo.CareWorker", "Description", c => c.String());
            DropColumn("dbo.CareWorker", "HasWWCC");
            DropColumn("dbo.CareWorker", "HasWWVPC");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CareWorker", "HasWWVPC", c => c.Boolean(nullable: false));
            AddColumn("dbo.CareWorker", "HasWWCC", c => c.Boolean(nullable: false));
            DropColumn("dbo.CareWorker", "Description");
            DropColumn("dbo.CareWorker", "ReligionID");
            DropColumn("dbo.CareWorker", "CultureID");
            DropColumn("dbo.CareWorker", "SecondLanguageID");
            DropColumn("dbo.CareWorker", "FirstLanguageID");
            DropColumn("dbo.CareWorker", "LocationCoverageRadius");
            DropColumn("dbo.CareWorker", "ProvisionLocationSuburbID");
        }
    }
}
