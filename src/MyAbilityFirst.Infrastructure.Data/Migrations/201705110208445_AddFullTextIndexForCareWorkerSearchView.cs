namespace MyAbilityFirst.Infrastructure.Data.Migrations
{
	using System.Data.Entity.Migrations;

	public partial class AddFullTextIndexForCareWorkerSearchView : DbMigration
	{
		public override void Up()
		{
			/* Full text search isn't supported on LocalDB, will use wildcard search for now.
			 * 
			Sql("CREATE FULLTEXT CATALOG ftIndicies AS DEFAULT;", true);
			Sql(@"
						CREATE FULLTEXT INDEX ON [vw_CareWorker_Search] (
							FirstName 
								TYPE COLUMN nvarchar(max)
								Language 'ENGLISH',
							LastName 
								TYPE COLUMN nvarchar(max)
								Language 'ENGLISH'
						)
						KEY INDEX [vw_CareWorker_Search_id]
						ON ftIndicies;
					",
			true);
			*/
		}

		public override void Down()
		{
			/*
				Sql("DROP FULLTEXT INDEX ON [vw_CareWorker_Search]");
				Sql("DROP FULLTEXT CATALOG ftIndicies");
			*/
		}
	}
}
