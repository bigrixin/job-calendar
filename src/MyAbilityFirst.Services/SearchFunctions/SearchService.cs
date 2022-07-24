using AutoMapper;
using MyAbilityFirst.Domain;
using MyAbilityFirst.Infrastructure;
using MyAbilityFirst.Models;
using PagedList;
using System.Data.Entity.Spatial;
using System.Data.SqlClient;
using System.Linq;

namespace MyAbilityFirst.Services.SearchFunctions
{
	public class SearchService : ISearchService
	{

		#region Fields

		private readonly IWriteEntities _entities;
		private IMapper _mapper;

		#endregion

		#region Ctor

		public SearchService(IWriteEntities entities, IMapper mapper)
		{
			this._entities = entities;
			this._mapper = mapper;
		}

		#endregion

		#region Build and Execute

		public PagedList<CareWorkerSearchResult> GetCareWorkerSearchResults(
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
		)
		{
			IQueryable<CareWorker> query = GetAllUsers<CareWorker>();

			if (PostedSubcategoryIDs != null && PostedSubcategoryIDs.Count() > 0)
				query = FilterUserByUserSubcategory<CareWorker>(query, PostedSubcategoryIDs);
			if (query == null) return null;

			if (!string.IsNullOrWhiteSpace(SearchTerm))
				query = FilterUserByName(query, SearchTerm);
			if (query == null) return null;

			if (Longitude != null && Latitude != null && radiusInKm != null && radiusInKm >= 0)
				query = FilterUserByDistance(query, Longitude, Latitude, radiusInKm);
			if (query == null) return null;

			if (PostedGenderIDs != null && PostedGenderIDs.Count() > 0)
				query = FilterCareWorkerByGender(query, PostedGenderIDs);
			if (query == null) return null;

			if (PostedLanguageIDs != null && PostedLanguageIDs.Count() > 0)
				query = FilterCareWorkerByLanguage(query, PostedLanguageIDs);
			if (query == null) return null;

			if (PostedCultureIDs != null && PostedCultureIDs.Count() > 0)
				query = FilterCareWorkerByCulture(query, PostedCultureIDs);
			if (query == null) return null;

			if (PostedReligionIDs != null && PostedReligionIDs.Count() > 0)
				query = FilterCareWorkerByReligion(query, PostedReligionIDs);
			if (query == null) return null;

			if (PostedPersonalityIDs != null && PostedPersonalityIDs.Count() > 0)
				query = FilterCareWorkerByPersonality(query, PostedPersonalityIDs);
			if (query == null) return null;

			IQueryable<CareWorkerSearchResult> res = ProjectQueryAsCareWorkerSearchResults(ownerUserID, query, Longitude, Latitude);

			bool firstSort = true;
			switch (sortOption)
			{
				case SortOption.MostRecent:
					res = OrderCareWorkerByDateModified(res, SortOrder.Descending, ref firstSort);
					break;
				case SortOption.Closest:
					if (Latitude != null && Longitude != null)
						res = OrderCareWorkerByDistanceFromLatLong(res, SortOrder.Ascending, Longitude, Latitude, ref firstSort);
					break;
				default:
					break;
			}
			return GetResultsAsPagedList(res, pageNumber, pageSize);
		}

		#endregion

		#region User filters

		private PagedList<T> GetResultsAsPagedList<T>(IQueryable<T> query, int pageNumber = 1, int pageSize = PageSize.DefaultSearchPageSize)
		{
			if (query == null)
				return null;

			if (pageNumber < 1)
				pageNumber = 1;

			if (pageSize < 1)
				pageSize = PageSize.DefaultSearchPageSize;

			if (pageSize > PageSize.MaxSearchPageSize)
				pageSize = PageSize.MaxSearchPageSize;

			return new PagedList<T>(query, (int)pageNumber, (int)pageSize);
		}

		private IQueryable<TUser> GetAllUsers<TUser>() where TUser: User
		{
			return _entities.GetLazy<TUser>();
		}

		private IQueryable<TUser> FilterUserByName<TUser>(IQueryable<TUser> query, string name) where TUser : User
		{
			return query.Where(u => u.FirstName.Contains(name) || u.LastName.Contains(name) || name.Contains(u.FirstName + " " + u.LastName));
		}

		private IQueryable<TUser> FilterUserByDistance<TUser>(IQueryable<TUser> query, decimal? longitude, decimal? latitude, float? radiusInKm) where TUser : User
		{
			return query.Where(u => DbGeography.FromText("POINT(" + longitude.ToString() + " " + latitude.ToString() + ")", 4326).Distance(u.Address.GeoPoint) / 1000 <= radiusInKm);
		}

		private IQueryable<TUser> FilterUserByUserSubcategory<TUser>(IQueryable<TUser> query, int[] subcategoryIDs) where TUser : User
		{
			if (subcategoryIDs == null || subcategoryIDs.Count() == 0)
				return null;
			return getUserIDsFromUserSubcategory(query, subcategoryIDs);
		}

		#endregion
		
		#region CareWorker filters

		private IQueryable<CareWorker> FilterCareWorkerByGender(IQueryable<CareWorker> query, int[] genderIDs)
		{
			if (genderIDs == null || genderIDs.Count() != 1)
				return null;
			int genderID = genderIDs[0];
			return query.Where(cw => cw.GenderID == genderID);
		}

		private IQueryable<CareWorker> FilterCareWorkerByLanguage(IQueryable<CareWorker> query, int[] languageIDs)
		{
			if (languageIDs == null || languageIDs.Count() < 1 || languageIDs.Count() > 2)
				return null;

			int languageID0 = languageIDs[0];
			
			if (languageIDs.Count() == 2) 
			{
				int languageID1 = languageIDs[1];
				return query.Where(cw =>
					cw.FirstLanguageID == languageID0 && cw.SecondLanguageID == languageID1 ||
					cw.FirstLanguageID == languageID1 && cw.SecondLanguageID == languageID0);
			}
			else 
			{
				return query.Where(cw =>
					cw.FirstLanguageID == languageID0 || cw.SecondLanguageID == languageID0);
			}
		}

		private IQueryable<CareWorker> FilterCareWorkerByCulture(IQueryable<CareWorker> query, int[] cultureIDs)
		{
			if (cultureIDs == null || cultureIDs.Count() != 1)
				return null;
			int cultureID = cultureIDs[0];
			return query.Where(cw => cw.CultureID == cultureID);
		}

		private IQueryable<CareWorker> FilterCareWorkerByReligion(IQueryable<CareWorker> query, int[] religionIDs)
		{
			if (religionIDs == null || religionIDs.Count() != 1)
				return null;
			int religionID = religionIDs[0];
			return query.Where(cw => cw.ReligionID == religionID);
		}

		private IQueryable<CareWorker> FilterCareWorkerByPersonality(IQueryable<CareWorker> query, int[] personalityIDs)
		{
			if (personalityIDs == null || personalityIDs.Count() != 1)
				return null;
			int personalityID = personalityIDs[0];
			return query.Where(cw => cw.PersonalityID == personalityID);
		}

		private IQueryable<CareWorkerSearchResult> OrderCareWorkerByDistanceFromLatLong(IQueryable<CareWorkerSearchResult> query, SortOrder sortOrder, decimal? longitude, decimal? latitude, ref bool firstSort)
		{
			if (longitude == null || latitude == null)
				return query;

			query = query.OrderBy(
				cw => DbGeography.FromText("POINT(" + longitude.ToString() + " " + latitude.ToString() + ")", 4326).Distance(cw.Address.GeoPoint) / 1000,
				sortOrder,
				ref firstSort
			);
			return query;
		}

		private IQueryable<CareWorkerSearchResult> OrderCareWorkerByDateModified(IQueryable<CareWorkerSearchResult> query, SortOrder sortOrder, ref bool firstSort)
		{
			query = query.OrderBy(
				cw => cw.UpdatedAt,
				sortOrder,
				ref firstSort
			);
			return query;
		}

		private IQueryable<CareWorkerSearchResult> ProjectQueryAsCareWorkerSearchResults(int ownerUserID, IQueryable<CareWorker> query, decimal? Longitude = null, decimal? Latitude = null)
		{

			IQueryable<Shortlist> shortlists = _entities.GetLazy<Shortlist>();
			shortlists.Where(sl => sl.OwnerUserID == ownerUserID);

			return
				from q in query
				from sl in shortlists
					.Where(sl2 => sl2.OwnerUserID == ownerUserID && q.ID == sl2.SelectedUserID && sl2.Selected).DefaultIfEmpty()
				select new CareWorkerSearchResult
				{
					CareWorkerID = q.ID,
					Address = q.Address,
					UpdatedAt = q.UpdatedAt,
					FirstName = q.FirstName,
					LastName = q.LastName,
					Description = q.Description,
					DistanceFromSearchInKm = (Latitude == null || Longitude == null) ? null : DbGeography.FromText("POINT(" + Longitude.ToString() + " " + Latitude.ToString() + ")", 4326).Distance(q.Address.GeoPoint) / 1000,
					WithinServiceCoverage = (Latitude == null || Longitude == null) ? true : q.LocationCoverageRadius >= DbGeography.FromText("POINT(" + Longitude.ToString() + " " + Latitude.ToString() + ")", 4326).Distance(q.Address.GeoPoint) / 1000,
					Longitude = q.Address.Longitude,
					Latitude = q.Address.Latitude,
					Shortlisted = (bool?) sl.Selected,
					PictureURL = "https://www.w3schools.com/bootstrap/img_avatar3.png"
				};
		}

		#endregion

		#region Helpers

		private IQueryable<TUser> getUserIDsFromUserSubcategory<TUser>(IQueryable<TUser> query, int[] subcategoryIDs) where TUser: User
		{
			return
				from q in query
				join userIDs in (
						from usc in _entities.GetLazy<UserSubcategory>()
						join sc in subcategoryIDs
							on usc.SubCategoryID equals sc
						group usc.OwnerUserID by usc.OwnerUserID into grp
						where grp.Count() == subcategoryIDs.Count()
						select grp.Key
				)
					on q.ID equals userIDs
				select q;
		}

		#endregion

	}
}
