using Autofac;

namespace MyAbilityFirst.Services.Common
{
	public class CommonModule : Autofac.Module
	{

		protected override void Load(ContainerBuilder builder)
		{
			base.Load(builder);

			// register presentation layer service
			builder
				.RegisterType<PresentationService>()
				.As<IPresentationService>();

			// register upload service
			builder
				.RegisterType<UploadService>()
				.As<IUploadService>();

			// register calendar service
			builder
				.RegisterType<CalendarService>()
				.As<ICalendarService>();

			// register job service
			builder
				.RegisterType<JobService>()
				.As<IJobService>();
		}
	}
}