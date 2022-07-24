namespace MyAbilityFirst.Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAddressObjectInUserTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "Address_Suburb", c => c.String());
            AddColumn("dbo.User", "Address_State", c => c.String());
            AddColumn("dbo.User", "Address_Postcode", c => c.Int(nullable: false));
            AddColumn("dbo.User", "Address_FullAddress", c => c.String());
            AddColumn("dbo.User", "Address_Latitude", c => c.Decimal(nullable: false, precision: 12, scale: 9));
            AddColumn("dbo.User", "Address_Longitude", c => c.Decimal(nullable: false, precision: 12, scale: 9));
            DropColumn("dbo.Client", "ProvisionLocationSuburbID");
            DropColumn("dbo.Patient", "ProvisionLocationSuburbID");
            DropColumn("dbo.User", "ResidentialSuburbID");
            DropTable("dbo.Suburb");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Suburb",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        State = c.String(),
                        Postcode = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.User", "ResidentialSuburbID", c => c.Int(nullable: false));
            AddColumn("dbo.Patient", "ProvisionLocationSuburbID", c => c.Int(nullable: false));
            AddColumn("dbo.Client", "ProvisionLocationSuburbID", c => c.Int(nullable: false));
            DropColumn("dbo.User", "Address_Longitude");
            DropColumn("dbo.User", "Address_Latitude");
            DropColumn("dbo.User", "Address_FullAddress");
            DropColumn("dbo.User", "Address_Postcode");
            DropColumn("dbo.User", "Address_State");
            DropColumn("dbo.User", "Address_Suburb");
        }
    }
}
