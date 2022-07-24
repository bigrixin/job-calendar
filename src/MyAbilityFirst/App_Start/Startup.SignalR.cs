using Owin;

namespace MyAbilityFirst
{
	public partial class Startup
	{
		public static void ConfigureSignalR(IAppBuilder app)
		{
			app.MapSignalR();
		}
	}
}