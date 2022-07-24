namespace MyAbilityFirst.Infrastructure.Data.Migrations
{
	using System.Data.Entity.Migrations;

	public partial class AddCareWorkerSearchView : DbMigration
	{
		public override void Up()
		{
			Sql(@"
						CREATE VIEW dbo.vw_CareWorker_Search
						WITH SCHEMABINDING
						AS   
						SELECT 
						u.id, u.FirstName, u.LastName, u.GenderID, u.DoB, u.Email, u.Address_Suburb, u.Address_State, u.Address_Postcode, u.Address_FullAddress, u.Address_Latitude, u.Address_Longitude, u.Phone, u.status, u.CreatedAt, u.UpdatedAt, u.LoginIdentityId,
						c.LocationCoverageRadius, c.FirstLanguageID, c.SecondLanguageID, c.CultureID, c.ReligionID, c.Description, c.GenderPreferenceID, c.PersonalityID
						FROM dbo.CareWorker c INNER JOIN dbo.[User] u on c.ID = u.ID
					");
			Sql("CREATE UNIQUE CLUSTERED INDEX vw_CareWorker_Search_id on dbo.vw_CareWorker_Search (id)");
		}

		public override void Down()
		{
			Sql(@"
						IF OBJECT_ID ('vw_CareWorker_Search', 'V') IS NOT NULL  
						DROP VIEW vw_CareWorker_Search"
			);
		}
	}
}


