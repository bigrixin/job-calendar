namespace MyAbilityFirst.Infrastructure.Data.Migrations
{
	using System.Data.Entity.Migrations;

	public partial class AddSpatialPointToAddress : DbMigration
	{
		public override void Up()
		{
			AddColumn("dbo.User", "Address_GeoPoint", c => c.Geography());
			AddColumn("dbo.Job", "Address_GeoPoint", c => c.Geography());
			Sql("Update dbo.[User] set Address_GeoPoint = Geography::Point(Address_Latitude, Address_Longitude, 4326)");
			Sql("Update dbo.[Job] set Address_GeoPoint = Geography::Point(Address_Latitude, Address_Longitude, 4326)");
		}

		public override void Down()
		{
			DropColumn("dbo.Job", "Address_GeoPoint");
			DropColumn("dbo.User", "Address_GeoPoint");
		}
	}
}
