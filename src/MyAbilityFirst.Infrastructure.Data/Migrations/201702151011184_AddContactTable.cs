namespace MyAbilityFirst.Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddContactTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Contact",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ContactName = c.String(),
                        MobilePhone = c.String(),
                        WorkPhone = c.String(),
                        HomePhone = c.String(),
                        Email = c.String(),
                        RelationshipID = c.Int(nullable: false),
                        PatientID = c.Int(nullable: false),
		})
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Patient", t => t.PatientID)
                .Index(t => t.PatientID);
            
            DropColumn("dbo.Patient", "Contact1_MobilePhone");
            DropColumn("dbo.Patient", "Contact1_WorkPhone");
            DropColumn("dbo.Patient", "Contact1_HomePhone");
            DropColumn("dbo.Patient", "Contact1_Email");
            DropColumn("dbo.Patient", "Contact1_RelationshipID");
            DropColumn("dbo.Patient", "Contact2_MobilePhone");
            DropColumn("dbo.Patient", "Contact2_WorkPhone");
            DropColumn("dbo.Patient", "Contact2_HomePhone");
            DropColumn("dbo.Patient", "Contact2_Email");
            DropColumn("dbo.Patient", "Contact2_RelationshipID");
            DropColumn("dbo.Patient", "Contact3_MobilePhone");
            DropColumn("dbo.Patient", "Contact3_WorkPhone");
            DropColumn("dbo.Patient", "Contact3_HomePhone");
            DropColumn("dbo.Patient", "Contact3_Email");
            DropColumn("dbo.Patient", "Contact3_RelationshipID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Patient", "Contact3_RelationshipID", c => c.Int());
            AddColumn("dbo.Patient", "Contact3_Email", c => c.String());
            AddColumn("dbo.Patient", "Contact3_HomePhone", c => c.String());
            AddColumn("dbo.Patient", "Contact3_WorkPhone", c => c.String());
            AddColumn("dbo.Patient", "Contact3_MobilePhone", c => c.String());
            AddColumn("dbo.Patient", "Contact2_RelationshipID", c => c.Int());
            AddColumn("dbo.Patient", "Contact2_Email", c => c.String());
            AddColumn("dbo.Patient", "Contact2_HomePhone", c => c.String());
            AddColumn("dbo.Patient", "Contact2_WorkPhone", c => c.String());
            AddColumn("dbo.Patient", "Contact2_MobilePhone", c => c.String());
            AddColumn("dbo.Patient", "Contact1_RelationshipID", c => c.Int());
            AddColumn("dbo.Patient", "Contact1_Email", c => c.String());
            AddColumn("dbo.Patient", "Contact1_HomePhone", c => c.String());
            AddColumn("dbo.Patient", "Contact1_WorkPhone", c => c.String());
            AddColumn("dbo.Patient", "Contact1_MobilePhone", c => c.String());
            DropForeignKey("dbo.Contact", "PatientID", "dbo.Patient");
            DropIndex("dbo.Contact", new[] { "PatientID" });
            DropTable("dbo.Contact");
        }
    }
}
