namespace MyAbilityFirst.Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSelectedFieldInShortlist : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shortlist", "Selected", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Shortlist", "Selected");
        }
    }
}
