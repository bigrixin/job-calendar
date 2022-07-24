namespace MyAbilityFirst.Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOrganistionTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Organisation",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        LogoURL = c.String(),
                        Address = c.String(),
                        Phone = c.String(),
                        Introduction = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Organisation");
        }
    }
}
