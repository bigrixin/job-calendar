namespace MyAbilityFirst.Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCareTypeIDToPatient : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Patient", "CareTypeID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Patient", "CareTypeID");
        }
    }
}
