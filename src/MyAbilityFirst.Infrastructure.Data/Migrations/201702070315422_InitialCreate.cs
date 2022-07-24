namespace MyAbilityFirst.Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.User",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        GenderID = c.Int(nullable: false),
                        DoB = c.DateTime(),
                        Email = c.String(),
                        ResidentialSuburbID = c.Int(nullable: false),
                        Phone = c.String(),
                        CreatedAt = c.DateTime(),
                        UpdatedAt = c.DateTime(),
                        LoginIdentityId = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.CareWorker",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        HasWWCC = c.Boolean(nullable: false),
                        HasWWVPC = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.User", t => t.ID)
                .Index(t => t.ID);
            
            CreateTable(
                "dbo.Client",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        ProvisionLocationSuburbID = c.Int(nullable: false),
                        Disclaimers_PremiseSafetyStatementAcknowledged = c.Boolean(nullable: false),
                        Disclaimers_FamilyFriendsPrivacyStatementAcknowledged = c.Boolean(nullable: false),
                        Disclaimers_OrganizationPrivacyStatementAcknowledged = c.Boolean(nullable: false),
                        Notifications_ReceiveEmailNotifications = c.Boolean(nullable: false),
                        Notifications_ReceiveSMSNotifications = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.User", t => t.ID)
                .Index(t => t.ID);
            
            CreateTable(
                "dbo.Patient",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        ClientID = c.Int(nullable: false),
                        ProvisionLocationSuburbID = c.Int(nullable: false),
                        Allergies = c.String(),
                        EmergencyResponse = c.String(),
                        FirstLanguageID = c.Int(nullable: false),
                        SecondLanguageID = c.Int(nullable: false),
                        CultureID = c.Int(nullable: false),
                        ReligionID = c.Int(nullable: false),
                        GoalsAndAspirations = c.String(),
                        CarePlanURL = c.String(),
                        NDISPlanURL = c.String(),
                        GPDocumentURL = c.String(),
                        BirthCertificateURL = c.String(),
                        MedicareURL = c.String(),
                        ProofOfAgeURL = c.String(),
                        PsyReportURL = c.String(),
                        ReviewAssessmentURL = c.String(),
                        Contact1_MobilePhone = c.String(),
                        Contact1_WorkPhone = c.String(),
                        Contact1_HomePhone = c.String(),
                        Contact1_Email = c.String(),
                        Contact1_RelationshipID = c.Int(),
                        Contact2_MobilePhone = c.String(),
                        Contact2_WorkPhone = c.String(),
                        Contact2_HomePhone = c.String(),
                        Contact2_Email = c.String(),
                        Contact2_RelationshipID = c.Int(),
                        Contact3_MobilePhone = c.String(),
                        Contact3_WorkPhone = c.String(),
                        Contact3_HomePhone = c.String(),
                        Contact3_Email = c.String(),
                        Contact3_RelationshipID = c.Int(),
                        HouseDetails_HasAdequateAccess = c.Boolean(nullable: false),
                        HouseDetails_HasSmokersLivingIn = c.Boolean(nullable: false),
                        HouseDetails_HasStreetParking = c.Boolean(nullable: false),
                        HouseDetails_Equipments = c.String(),
                        HouseDetails_SpecialInstructions = c.String(),
                        MedicationDetails_MedicationName = c.String(),
                        MedicationDetails_Dosage = c.String(),
                        MedicationDetails_Frequency = c.String(),
                        MedicationDetails_MedicationReason = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.User", t => t.ID)
                .Index(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Patient", "ID", "dbo.User");
            DropForeignKey("dbo.Client", "ID", "dbo.User");
            DropForeignKey("dbo.CareWorker", "ID", "dbo.User");
            DropIndex("dbo.Patient", new[] { "ID" });
            DropIndex("dbo.Client", new[] { "ID" });
            DropIndex("dbo.CareWorker", new[] { "ID" });
            DropTable("dbo.Patient");
            DropTable("dbo.Client");
            DropTable("dbo.CareWorker");
            DropTable("dbo.User");
        }
    }
}
