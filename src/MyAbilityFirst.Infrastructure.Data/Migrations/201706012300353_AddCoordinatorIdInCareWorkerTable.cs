namespace MyAbilityFirst.Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCoordinatorIdInCareWorkerTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CareWorker", "CoordinatorID", c => c.Int());
            AddColumn("dbo.CareWorker", "ApprovedDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CareWorker", "ApprovedDate");
            DropColumn("dbo.CareWorker", "CoordinatorID");
        }
    }
}
