namespace MyAbilityFirst.Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCoordinatorTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Coordinator",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        OrganisationId = c.Int(nullable: false),
                        StartingDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.User", t => t.ID)
                .Index(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Coordinator", "ID", "dbo.User");
            DropIndex("dbo.Coordinator", new[] { "ID" });
            DropTable("dbo.Coordinator");
        }
    }
}
