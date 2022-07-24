namespace MyAbilityFirst.Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEmploymentFormalEducationTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmploymentFormalEducation",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FormalEducationID = c.Int(nullable: false),
                        Description = c.String(),
                        CareWorker_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CareWorker", t => t.CareWorker_ID)
                .Index(t => t.CareWorker_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EmploymentFormalEducation", "CareWorker_ID", "dbo.CareWorker");
            DropIndex("dbo.EmploymentFormalEducation", new[] { "CareWorker_ID" });
            DropTable("dbo.EmploymentFormalEducation");
        }
    }
}
