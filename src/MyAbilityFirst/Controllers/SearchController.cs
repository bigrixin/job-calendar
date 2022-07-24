using System.Web.Mvc;
using AutoMapper;
using MyAbilityFirst.Services.SearchFunctions;
using MyAbilityFirst.Models;

namespace MyAbilityFirst.Controllers
{
	[Authorize]
	public class SearchController : Controller
	{

		#region Fields

		private readonly IMapper _mapper;
		private readonly ISearchService _searchServices;

		#endregion

		#region Ctor

		public SearchController(IMapper mapper, ISearchService searchServices)
		{
			this._mapper = mapper;
			this._searchServices = searchServices;
		}

		#endregion

		#region Actions
		[Route("search/SearchCareWorker")]
		public ActionResult SearchCareWorker(SearchViewModel vm)
		{
			if (ModelState.IsValid)
			{
				vm.UserID = this.GetLoggedInUser().ID;
				vm = _mapper.Map(vm, vm);
				if (vm.SearchTerm != null || vm.Longitude != null || vm.Latitude != null || vm.RadiusInKm != null || vm.PostedSubcategoryIDs != null || vm.PostedGenderIDs != null || vm.PostedLanguageIDs != null || vm.PostedCultureIDs != null || vm.PostedReligionIDs != null || vm.PostedPersonalityIDs != null)
				{
					vm.SearchResults = _searchServices.GetCareWorkerSearchResults(
						vm.UserID,
						vm.SearchTerm,
						vm.Longitude,
						vm.Latitude,
						vm.RadiusInKm,
						vm.PostedSubcategoryIDs,
						vm.PostedGenderIDs,
						vm.PostedLanguageIDs,
						vm.PostedCultureIDs,
						vm.PostedReligionIDs,
						vm.PostedPersonalityIDs,
						vm.SortOption,
						vm.PageNumber,
						vm.PageSize
					);
				}
				return View(vm);
			}
			return RedirectToAction("SearchCareWorker");
		}

		[HttpPost]
		public ActionResult SearchCareWorkerPaged(SearchViewModel vm)
		{
			vm.UserID = this.GetLoggedInUser().ID;
			vm.SearchResults = _searchServices.GetCareWorkerSearchResults(
				vm.UserID,
				vm.SearchTerm,
				vm.Longitude,
				vm.Latitude,
				vm.RadiusInKm,
				vm.PostedSubcategoryIDs,
				vm.PostedGenderIDs,
				vm.PostedLanguageIDs,
				vm.PostedCultureIDs,
				vm.PostedReligionIDs,
				vm.PostedPersonalityIDs,
				vm.SortOption,
				vm.PageNumber,
				vm.PageSize
			);
			return PartialView("_SearchResults", vm);
		}

		[HttpGet]
		public JsonResult SearchCareWorkerPagedJson(
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
			SortOption SortOption = SortOption.Closest,
			int PageNumber = 1,
			int PageSize = PageSize.DefaultSearchPageSize
		)
		{
			var res = Json(
				_searchServices.GetCareWorkerSearchResults(
					this.GetLoggedInUser().ID,
					SearchTerm,
					Longitude,
					Latitude,
					radiusInKm,
					PostedSubcategoryIDs,
					PostedGenderIDs,
					PostedLanguageIDs,
					PostedCultureIDs,
					PostedReligionIDs,
					PostedPersonalityIDs,
					SortOption,
					PageNumber,
					PageSize
				)
			);
			res.ContentEncoding = System.Text.Encoding.UTF8;
			res.ContentType = "application/json";
			res.MaxJsonLength = 100000;
			res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
			res.RecursionLimit = 10;
			return res;
		}
		#endregion

	}

}