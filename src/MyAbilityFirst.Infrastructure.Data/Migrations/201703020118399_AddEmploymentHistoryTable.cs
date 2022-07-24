namespace MyAbilityFirst.Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEmploymentHistoryTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmploymentHistory",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CompanyName = c.String(),
                        PositionTitle = c.String(),
                        StartMonth = c.DateTime(),
                        EndMonth = c.DateTime(),
                        Description = c.String(),
                        CareWorker_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CareWorker", t => t.CareWorker_ID)
                .Index(t => t.CareWorker_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EmploymentHistory", "CareWorker_ID", "dbo.CareWorker");
            DropIndex("dbo.EmploymentHistory", new[] { "CareWorker_ID" });
            DropTable("dbo.EmploymentHistory");
        }
    }
}
