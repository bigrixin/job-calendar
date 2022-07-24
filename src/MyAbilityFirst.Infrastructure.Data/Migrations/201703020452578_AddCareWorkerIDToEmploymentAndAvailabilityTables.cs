namespace MyAbilityFirst.Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCareWorkerIDToEmploymentAndAvailabilityTables : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Availability", new[] { "CareWorker_ID" });
            DropIndex("dbo.EmploymentAchievement", new[] { "CareWorker_ID" });
            DropIndex("dbo.EmploymentFormalEducation", new[] { "CareWorker_ID" });
            DropIndex("dbo.EmploymentReference", new[] { "CareWorker_ID" });
            DropIndex("dbo.EmploymentHistory", new[] { "CareWorker_ID" });
            RenameColumn(table: "dbo.Availability", name: "CareWorker_ID", newName: "CareWorkerID");
            RenameColumn(table: "dbo.EmploymentAchievement", name: "CareWorker_ID", newName: "CareWorkerID");
            RenameColumn(table: "dbo.EmploymentFormalEducation", name: "CareWorker_ID", newName: "CareWorkerID");
            RenameColumn(table: "dbo.EmploymentReference", name: "CareWorker_ID", newName: "CareWorkerID");
            RenameColumn(table: "dbo.EmploymentHistory", name: "CareWorker_ID", newName: "CareWorkerID");
            AlterColumn("dbo.Availability", "CareWorkerID", c => c.Int(nullable: false));
            AlterColumn("dbo.EmploymentAchievement", "CareWorkerID", c => c.Int(nullable: false));
            AlterColumn("dbo.EmploymentFormalEducation", "CareWorkerID", c => c.Int(nullable: false));
            AlterColumn("dbo.EmploymentReference", "CareWorkerID", c => c.Int(nullable: false));
            AlterColumn("dbo.EmploymentHistory", "CareWorkerID", c => c.Int(nullable: false));
            CreateIndex("dbo.Availability", "CareWorkerID");
            CreateIndex("dbo.EmploymentAchievement", "CareWorkerID");
            CreateIndex("dbo.EmploymentFormalEducation", "CareWorkerID");
            CreateIndex("dbo.EmploymentReference", "CareWorkerID");
            CreateIndex("dbo.EmploymentHistory", "CareWorkerID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.EmploymentHistory", new[] { "CareWorkerID" });
            DropIndex("dbo.EmploymentReference", new[] { "CareWorkerID" });
            DropIndex("dbo.EmploymentFormalEducation", new[] { "CareWorkerID" });
            DropIndex("dbo.EmploymentAchievement", new[] { "CareWorkerID" });
            DropIndex("dbo.Availability", new[] { "CareWorkerID" });
            AlterColumn("dbo.EmploymentHistory", "CareWorkerID", c => c.Int());
            AlterColumn("dbo.EmploymentReference", "CareWorkerID", c => c.Int());
            AlterColumn("dbo.EmploymentFormalEducation", "CareWorkerID", c => c.Int());
            AlterColumn("dbo.EmploymentAchievement", "CareWorkerID", c => c.Int());
            AlterColumn("dbo.Availability", "CareWorkerID", c => c.Int());
            RenameColumn(table: "dbo.EmploymentHistory", name: "CareWorkerID", newName: "CareWorker_ID");
            RenameColumn(table: "dbo.EmploymentReference", name: "CareWorkerID", newName: "CareWorker_ID");
            RenameColumn(table: "dbo.EmploymentFormalEducation", name: "CareWorkerID", newName: "CareWorker_ID");
            RenameColumn(table: "dbo.EmploymentAchievement", name: "CareWorkerID", newName: "CareWorker_ID");
            RenameColumn(table: "dbo.Availability", name: "CareWorkerID", newName: "CareWorker_ID");
            CreateIndex("dbo.EmploymentHistory", "CareWorker_ID");
            CreateIndex("dbo.EmploymentReference", "CareWorker_ID");
            CreateIndex("dbo.EmploymentFormalEducation", "CareWorker_ID");
            CreateIndex("dbo.EmploymentAchievement", "CareWorker_ID");
            CreateIndex("dbo.Availability", "CareWorker_ID");
        }
    }
}
