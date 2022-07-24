namespace MyAbilityFirst.Infrastructure.Data.Migrations
{
	using System;
	using System.Data.Entity.Migrations;

	public partial class AddMessageInBookingTable : DbMigration
	{
		public override void Up()
		{
			AddColumn("dbo.Booking", "Message", c => c.String());
		}

		public override void Down()
		{
			DropColumn("dbo.Booking", "Message");
		}
	}
}
