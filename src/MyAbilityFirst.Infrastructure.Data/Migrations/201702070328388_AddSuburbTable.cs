namespace MyAbilityFirst.Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSuburbTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Suburb",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        State = c.String(),
                        Postcode = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Suburb");
        }
    }
}
