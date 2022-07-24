using Autofac;

namespace MyAbilityFirst.Infrastructure.Auth
{
	public class AuthServicesModule : Autofac.Module
	{

		protected override void Load(ContainerBuilder builder)
		{
			base.Load(builder);

			// register LoginService
			builder
					.RegisterType<LoginService>()
					.As<ILoginService>();
		}
	}
}