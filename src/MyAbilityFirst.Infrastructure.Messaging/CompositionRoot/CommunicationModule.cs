using Autofac;

namespace MyAbilityFirst.Infrastructure.Communication
{
	public class CommunicationModule : Autofac.Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			base.Load(builder);

			// register BroadcastHub
			builder
					.RegisterType<BroadcastService>()
						.As<IBroadcastService>().SingleInstance();

			// register EmailService
			builder
					.RegisterType<MandrillService>()
					.AsImplementedInterfaces();
		}
	}
}
