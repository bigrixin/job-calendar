using Autofac;

namespace MyAbilityFirst.Services.CoordinatorFunctions
{
	public class CoordinatorFunctionsModule : Autofac.Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			base.Load(builder);

			// register CoordinatorService
			builder
					.RegisterType<CoordinatorService>()
					.As<ICoordinatorService>();
		}
	}
}
