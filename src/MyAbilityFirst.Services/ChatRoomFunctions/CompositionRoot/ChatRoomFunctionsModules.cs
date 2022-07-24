using Autofac;

namespace MyAbilityFirst.Services.ChatRoomFunctions
{
	public class ChatRoomFunctionsModule : Autofac.Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			base.Load(builder);

			// register Chat Services
			builder
					.RegisterType<ChatRoomService>()
					.As<IChatRoomService>();
		}
	}
}
