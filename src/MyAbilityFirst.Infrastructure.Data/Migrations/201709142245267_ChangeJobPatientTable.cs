namespace MyAbilityFirst.Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeJobPatientTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Patient", "Job_ID", "dbo.Job");
            DropIndex("dbo.Patient", new[] { "Job_ID" });
            CreateTable(
                "dbo.PatientJob",
                c => new
                    {
                        Patient_ID = c.Int(nullable: false),
                        Job_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Patient_ID, t.Job_ID })
                .ForeignKey("dbo.Patient", t => t.Patient_ID, cascadeDelete: true)
                .ForeignKey("dbo.Job", t => t.Job_ID, cascadeDelete: true)
                .Index(t => t.Patient_ID)
                .Index(t => t.Job_ID);
            
            DropColumn("dbo.Patient", "Job_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Patient", "Job_ID", c => c.Int());
            DropForeignKey("dbo.PatientJob", "Job_ID", "dbo.Job");
            DropForeignKey("dbo.PatientJob", "Patient_ID", "dbo.Patient");
            DropIndex("dbo.PatientJob", new[] { "Job_ID" });
            DropIndex("dbo.PatientJob", new[] { "Patient_ID" });
            DropTable("dbo.PatientJob");
            CreateIndex("dbo.Patient", "Job_ID");
            AddForeignKey("dbo.Patient", "Job_ID", "dbo.Job", "ID");
        }
    }
}
