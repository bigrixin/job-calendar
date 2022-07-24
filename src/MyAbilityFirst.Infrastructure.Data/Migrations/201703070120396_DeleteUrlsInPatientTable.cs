namespace MyAbilityFirst.Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteUrlsInPatientTable : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Patient", "CarePlanURL");
            DropColumn("dbo.Patient", "NDISPlanURL");
            DropColumn("dbo.Patient", "GPDocumentURL");
            DropColumn("dbo.Patient", "BirthCertificateURL");
            DropColumn("dbo.Patient", "MedicareURL");
            DropColumn("dbo.Patient", "ProofOfAgeURL");
            DropColumn("dbo.Patient", "PsyReportURL");
            DropColumn("dbo.Patient", "ReviewAssessmentURL");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Patient", "ReviewAssessmentURL", c => c.String());
            AddColumn("dbo.Patient", "PsyReportURL", c => c.String());
            AddColumn("dbo.Patient", "ProofOfAgeURL", c => c.String());
            AddColumn("dbo.Patient", "MedicareURL", c => c.String());
            AddColumn("dbo.Patient", "BirthCertificateURL", c => c.String());
            AddColumn("dbo.Patient", "GPDocumentURL", c => c.String());
            AddColumn("dbo.Patient", "NDISPlanURL", c => c.String());
            AddColumn("dbo.Patient", "CarePlanURL", c => c.String());
        }
    }
}
