using Autofac;

namespace MyAbilityFirst.Services.SearchFunctions
{
	public class SearchFunctionsModule : Autofac.Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			base.Load(builder);

			// register SearchFunctionServices
			builder
					.RegisterType<SearchService>()
					.As<ISearchService>();
		}
	}
}
