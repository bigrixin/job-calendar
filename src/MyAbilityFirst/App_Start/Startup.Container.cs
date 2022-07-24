using Autofac;
using Autofac.Integration.Mvc;
using MyAbilityFirst.Domain;
using MyAbilityFirst.Infrastructure;
using MyAbilityFirst.Infrastructure.Auth;
using MyAbilityFirst.Infrastructure.Data;
using MyAbilityFirst.Services;
using MyAbilityFirst.Services.AttachmentManagement;
using MyAbilityFirst.Services.CareWorkerFunctions;
using MyAbilityFirst.Services.ClientFunctions;
using MyAbilityFirst.Services.Common;
using MyAbilityFirst.Services.ChatRoomFunctions;
using MyAbilityFirst.Services.SearchFunctions;
using Owin;
using System.Reflection;
using System.Web.Mvc;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;
using MyAbilityFirst.Infrastructure.Communication;
using MyAbilityFirst.Services.CoordinatorFunctions;
using Autofac.Integration.WebApi;
using System.Web.Http;

namespace MyAbilityFirst
{
	/// <summary>
	/// Register types into the Autofac Inversion of Control (IOC) container. Autofac makes it easy to register common 
	/// MVC types like the <see cref="UrlHelper"/> using the <see cref="AutofacWebTypesModule"/>. Feel free to change 
	/// this to another IoC container of your choice but ensure that common MVC types like <see cref="UrlHelper"/> are 
	/// registered. See http://autofac.readthedocs.org/en/latest/integration/aspnet.html.
	/// </summary>
	public partial class Startup
	{

		public static void ConfigureContainer(IAppBuilder app)
		{
			IContainer container = CreateContainer();
			app.UseAutofacMiddleware(container);

			// Register MVC Types 
			app.UseAutofacMvc();
		}

		private static IContainer CreateContainer()
		{
			ContainerBuilder builder = new ContainerBuilder();
			Assembly assembly = Assembly.GetExecutingAssembly();

			RegisterServices(builder);
			RegisterMvc(builder, assembly);
			RegisterSignalR(builder);

			IContainer container = builder.Build();

			SetMvcDependencyResolver(container);

			return container;
		}

		private static void RegisterServices(ContainerBuilder builder)
		{
			builder.RegisterType<BrowserConfigService>().As<IBrowserConfigService>().InstancePerRequest();
			builder.RegisterType<LoggingService>().As<ILoggingService>().SingleInstance();
			builder.RegisterType<ManifestService>().As<IManifestService>().InstancePerRequest();
			builder.RegisterType<OpenSearchService>().As<IOpenSearchService>().InstancePerRequest();
			builder.RegisterType<RobotsService>().As<IRobotsService>().InstancePerRequest();
			builder.RegisterType<SitemapService>().As<ISitemapService>().InstancePerRequest();
			builder.RegisterType<SitemapPingerService>().As<ISitemapPingerService>().InstancePerRequest();
			
			builder.RegisterType<PresentationService>().As<IPresentationService>().SingleInstance();
			builder.RegisterType<CacheService>().As<ICacheService>().SingleInstance();

			// register EF module
			builder.RegisterModule<EntityFrameworkModule>();

			// register ASP.NET Identity module
			builder.RegisterModule<AspNetIdentityModule>();
			builder.RegisterModule<AuthServicesModule>();

			// register business logic/services
			builder.RegisterModule<UserModule>();
			builder.RegisterModule<CommonModule>();
			builder.RegisterModule<ClientFunctionsModule>();
			builder.RegisterModule<CareWorkerFunctionsModule>();
			builder.RegisterModule<AttachmentManagementModule>();
			builder.RegisterModule<AutoMapperModule>();
			builder.RegisterModule<SearchFunctionsModule>();
			builder.RegisterModule<CoordinatorFunctionsModule>();
			builder.RegisterModule<ChatRoomFunctionsModule>();
			builder.RegisterModule<CommunicationModule>();
		}

		private static void RegisterMvc(ContainerBuilder builder, Assembly assembly)
		{
			// Register Common MVC Types
			builder.RegisterModule<AutofacWebTypesModule>();

			// Register MVC Filters
			builder.RegisterFilterProvider();

			// Register MVC Controllers
			builder.RegisterControllers(assembly);

			builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
		}

		private static void RegisterSignalR(ContainerBuilder builder)
		{
			var config = new HubConfiguration();
			builder.RegisterInstance(config.Resolver.Resolve<IConnectionManager>());
		}

		/// <summary>
		/// Sets the ASP.NET MVC dependency resolver.
		/// </summary>
		/// <param name="container">The container.</param>
		private static void SetMvcDependencyResolver(IContainer container)
		{
			DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
			GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver((IContainer)container);
		}

	}
}
