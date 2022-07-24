namespace MyAbilityFirst.Infrastructure.Data.Migrations
{
	using System.Data.Entity.Migrations;
	using System.IO;
	using System.Linq;
	using System.Reflection;

	public partial class AddElmahObjects : DbMigration
	{
		public override void Up()
		{
			var assembly = Assembly.GetExecutingAssembly();
			var elmahScript =
				assembly
					.GetManifestResourceNames()
					.Where(x => x.StartsWith("MyAbilityFirst.Infrastructure.Data.Sql.Elmah.SqlServer.sql"));

			if (elmahScript.Any())
			{
				var resourceName = elmahScript.FirstOrDefault();
				using (Stream stream = assembly.GetManifestResourceStream(resourceName))
				using (StreamReader reader = new StreamReader(stream))
				{
					string scriptText = reader.ReadToEnd();
					Sql(scriptText);
				}
			}
		}

		public override void Down()
		{
			DropStoredProcedure("dbo.ELMAH_GetErrorsXml");
			DropStoredProcedure("dbo.ELMAH_GetErrorXml");
			DropStoredProcedure("dbo.ELMAH_LogError");
			DropIndex("dbo.ELMAH_Error", "dbo.IX_ELMAH_Error_App_Time_Seq");
			DropTable("dbo.ELMAH_Error");
		}
	}
}