using Autofac;

namespace MyAbilityFirst.Services.AttachmentManagement
{
	public class AttachmentManagementModule : Module
	{

		protected override void Load(ContainerBuilder builder)
		{
			base.Load(builder);

			// register AttachmentService
			builder
					.RegisterType<AttachmentService>()
					.AsImplementedInterfaces();
		}
	}
}