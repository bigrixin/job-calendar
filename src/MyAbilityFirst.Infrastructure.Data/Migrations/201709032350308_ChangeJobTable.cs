namespace MyAbilityFirst.Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeJobTable : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Job", new[] { "ClientId" });
            AddColumn("dbo.Patient", "Job_ID", c => c.Int());
            AddColumn("dbo.Patient", "PatientId", c => c.Int(nullable: false));
            AddColumn("dbo.Job", "PreferredGenderID", c => c.Int(nullable: false));
            CreateIndex("dbo.Schedule", "JobID");
            CreateIndex("dbo.Job", "ClientID");
            CreateIndex("dbo.Patient", "Job_ID");
            AddForeignKey("dbo.Schedule", "JobID", "dbo.Job", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Patient", "Job_ID", "dbo.Job", "ID");
            DropColumn("dbo.Job", "GenderId");
            DropColumn("dbo.Job", "ServiceId");
            DropColumn("dbo.Job", "ServiceAt");
            DropColumn("dbo.Job", "PictureURL");
            DropColumn("dbo.Job", "PatientId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Job", "PatientId", c => c.Int(nullable: false));
            AddColumn("dbo.Job", "PictureURL", c => c.String());
            AddColumn("dbo.Job", "ServiceAt", c => c.DateTime());
            AddColumn("dbo.Job", "ServiceId", c => c.Int(nullable: false));
            AddColumn("dbo.Job", "GenderId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Patient", "Job_ID", "dbo.Job");
            DropForeignKey("dbo.Schedule", "JobID", "dbo.Job");
            DropIndex("dbo.Patient", new[] { "Job_ID" });
            DropIndex("dbo.Job", new[] { "ClientID" });
            DropIndex("dbo.Schedule", new[] { "JobID" });
            DropColumn("dbo.Job", "PreferredGenderID");
            DropColumn("dbo.Patient", "PatientId");
            DropColumn("dbo.Patient", "Job_ID");
            CreateIndex("dbo.Job", "ClientId");
        }
    }
}
