namespace MyAbilityFirst.Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPictureURLToJob : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Job", "PictureURL", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Job", "PictureURL");
        }
    }
}
