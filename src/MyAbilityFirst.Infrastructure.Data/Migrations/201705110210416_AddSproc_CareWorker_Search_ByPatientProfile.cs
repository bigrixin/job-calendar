namespace MyAbilityFirst.Infrastructure.Data.Migrations
{
	using System.Data.Entity.Migrations;

	public partial class AddSproc_CareWorker_Search_ByPatientProfile : DbMigration
	{
		public override void Up()
		{
			CreateStoredProcedure(
				"dbo.sproc_CareWorker_Search_ByPatientProfile",
				p => new
				{
					PatientID = p.String()
				},
				body:
					@"
						SELECT DISTINCT matchingCareWorkers.careWorkerID 
						FROM (

							SELECT cw.OwnerUserID as 'careWorkerID'
							FROM UserSubcategory p 
							inner join UserSubcategory cw 
							ON p.OwnerUserID = @patientID and cw.OwnerUserID != @patientID and p.SubcategoryID = cw.SubcategoryID 
							inner join Subcategory sc
							ON p.SubcategoryID = sc.ID
							inner join Category c
							ON c.Name != 'Gender' and sc.CategoryID = c.ID 

							UNION ALL 

							SELECT cw.ID as 'careWorkerID'
							FROM Patient p 
							inner join [User] pu 
							ON p.ID = pu.ID
							inner join CareWorker cw 
							ON p.id = @patientID and (
								p.FirstLanguageID = cw.FirstLanguageID or p.SecondLanguageID = cw.SecondLanguageID 
								or p.FirstLanguageID = cw.SecondLanguageID or p.SecondLanguageID = cw.FirstLanguageID
								or p.CultureID = cw.CultureID or p.ReligionID = cw.ReligionID
								or pu.GenderID = cw.GenderPreferenceID
							)

						) as matchingCareWorkers
					"
			);
		}

		public override void Down()
		{
			DropStoredProcedure("dbo.sproc_CareWorker_Search_ByPatientProfile");
		}

	}
}

