namespace MyAbilityFirst.Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCategorySubcategoryTableForUserPreferences : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Category",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Subcategory",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CategoryID = c.Int(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID)
								.ForeignKey("dbo.Category", t => t.CategoryID);
            
            CreateTable(
                "dbo.UserSubcategory",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        SubCategoryID = c.Int(nullable: false),
                        CustomValue = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserSubcategory");
            DropTable("dbo.Subcategory");
            DropTable("dbo.Category");
        }
    }
}
