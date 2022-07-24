namespace MyAbilityFirst.Infrastructure.Data.Migrations
{
	using System.Data.Entity.Migrations;

	public partial class AddSpatialIndexingForGeoPoint : DbMigration
	{
		public override void Up()
		{
			Sql(@"
					CREATE SPATIAL INDEX SIndx_User_GeoPoint  
					ON dbo.[User](Address_GeoPoint); 
				");

			Sql(@"
					CREATE SPATIAL INDEX SIndx_Job_GeoPoint  
					ON dbo.[Job](Address_GeoPoint); 
				");
		}
		public override void Down()
		{
			Sql("DROP INDEX SIndx_Job_GeoPoint ON dbo.[Job];");
			Sql("DROP INDEX SIndx_User_GeoPoint ON dbo.[User];");
		}
	}
}
