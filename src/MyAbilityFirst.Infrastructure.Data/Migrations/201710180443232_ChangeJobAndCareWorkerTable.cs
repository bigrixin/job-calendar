namespace MyAbilityFirst.Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeJobAndCareWorkerTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.JobCareWorker",
                c => new
                    {
                        Job_ID = c.Int(nullable: false),
                        CareWorker_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Job_ID, t.CareWorker_ID })
                .ForeignKey("dbo.Job", t => t.Job_ID, cascadeDelete: true)
                .ForeignKey("dbo.CareWorker", t => t.CareWorker_ID, cascadeDelete: true)
                .Index(t => t.Job_ID)
                .Index(t => t.CareWorker_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JobCareWorker", "CareWorker_ID", "dbo.CareWorker");
            DropForeignKey("dbo.JobCareWorker", "Job_ID", "dbo.Job");
            DropIndex("dbo.JobCareWorker", new[] { "CareWorker_ID" });
            DropIndex("dbo.JobCareWorker", new[] { "Job_ID" });
            DropTable("dbo.JobCareWorker");
        }
    }
}
