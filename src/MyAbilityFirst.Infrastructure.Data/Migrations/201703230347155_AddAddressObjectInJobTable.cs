namespace MyAbilityFirst.Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAddressObjectInJobTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Job", "Address_Suburb", c => c.String());
            AddColumn("dbo.Job", "Address_State", c => c.String());
            AddColumn("dbo.Job", "Address_Postcode", c => c.Int(nullable: false));
            AddColumn("dbo.Job", "Address_FullAddress", c => c.String());
            AddColumn("dbo.Job", "Address_Latitude", c => c.Decimal(nullable: false, precision: 12, scale: 9));
            AddColumn("dbo.Job", "Address_Longitude", c => c.Decimal(nullable: false, precision: 12, scale: 9));
            DropColumn("dbo.Job", "SuburbId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Job", "SuburbId", c => c.Int(nullable: false));
            DropColumn("dbo.Job", "Address_Longitude");
            DropColumn("dbo.Job", "Address_Latitude");
            DropColumn("dbo.Job", "Address_FullAddress");
            DropColumn("dbo.Job", "Address_Postcode");
            DropColumn("dbo.Job", "Address_State");
            DropColumn("dbo.Job", "Address_Suburb");
        }
    }
}
