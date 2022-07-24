using Autofac;

namespace MyAbilityFirst.Services.CareWorkerFunctions
{
	public class CareWorkerFunctionsModule : Autofac.Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			base.Load(builder);

			// register CareWorkerService
			builder
					.RegisterType<CareWorkerService>()
					.As<ICareWorkerService>();
		}
	}
}
