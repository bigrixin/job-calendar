namespace MyAbilityFirst.Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEmploymentEmploymentAchievementTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmploymentAchievement",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        CareWorker_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CareWorker", t => t.CareWorker_ID)
                .Index(t => t.CareWorker_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EmploymentAchievement", "CareWorker_ID", "dbo.CareWorker");
            DropIndex("dbo.EmploymentAchievement", new[] { "CareWorker_ID" });
            DropTable("dbo.EmploymentAchievement");
        }
    }
}
