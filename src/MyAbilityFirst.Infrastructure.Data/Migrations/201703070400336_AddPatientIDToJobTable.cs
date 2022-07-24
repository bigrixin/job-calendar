namespace MyAbilityFirst.Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPatientIDToJobTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Job", "PatientId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Job", "PatientId");
        }
    }
}
