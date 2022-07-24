using PagedList;
using MyAbilityFirst.Models;

namespace MyAbilityFirst.Services.SearchFunctions
{
	public interface ISearchService
	{
		PagedList<CareWorkerSearchResult> GetCareWorkerSearchResults(
			int ownerUserID,
			string SearchTerm = "",
			decimal? Longitude = null,
			decimal? Latitude = null,
			float? radiusInKm = 0,
			int[] PostedSubcategoryIDs = null,
			int[] PostedGenderIDs = null,
			int[] PostedLanguageIDs = null,
			int[] PostedCultureIDs = null,
			int[] PostedReligionIDs = null,
			int[] PostedPersonalityIDs = null,
			SortOption sortOption = SortOption.Closest,
			int pageNumber = 1,
			int pageSize = PageSize.DefaultSearchPageSize
		);
	}
}
