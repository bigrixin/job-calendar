using MyAbilityFirst.Domain;
using PagedList;
using System.Collections.Generic;

namespace MyAbilityFirst.Models
{
	public class SearchViewModel
	{
		// User Location Details
		public int UserID { get; set; }
		public decimal? HomeLongitude { get; set; }
		public decimal? HomeLatitude { get; set; }

		// Search Parameters
		public string SearchURL { get; set; }
		public string SearchTerm { get; set; }
		public decimal? Longitude { get; set; }
		public decimal? Latitude { get; set; }
		public float? RadiusInKm { get; set; }
		public int[] PostedSubcategoryIDs { get; set; }
		public int[] PostedGenderIDs { get; set; }
		public int[] PostedLanguageIDs { get; set; }
		public int[] PostedCultureIDs { get; set; }
		public int[] PostedReligionIDs { get; set; }
		public int[] PostedPersonalityIDs { get; set; }
		public SortOption SortOption { get; set; }

		// Search Results
		public PagedList<CareWorkerSearchResult> SearchResults { get; set; }
		public int PageNumber { get; set; }
		public int PageSize { get; set; }

		// multi selects
		public List<Subcategory> GenderList { get; set; }
		public List<Subcategory> LanguageList { get; set; }
		public List<Subcategory> CultureList { get; set; }
		public List<Subcategory> ReligionList { get; set; }
		public List<Subcategory> PersonalityList { get; set; }
		public List<Subcategory> QualificationTypeList { get; set; }
		public List<Subcategory> NursingQualificationsList { get; set; }
		public List<Subcategory> PersonalCareQualificationsList { get; set; }
		public List<Subcategory> OtherQualificationsList { get; set; }
		public List<Subcategory> VerificationChecksList { get; set; }
		public List<Subcategory> OtherPreferenceList { get; set; }
		public List<Subcategory> DisabilityCareExperienceList { get; set; }
		public List<Subcategory> MentalHealthCareExperienceList { get; set; }
		public List<Subcategory> AgedCareExperienceList { get; set; }
		public List<Subcategory> ChronicMedicalConditionsCareExperienceList { get; set; }
		public List<Subcategory> InterestList { get; set; }

		// for tracking changes
		public List<Subcategory> PreviousGenderList { get; set; }
		public List<Subcategory> PreviousLanguageList { get; set; }
		public List<Subcategory> PreviousCultureList { get; set; }
		public List<Subcategory> PreviousReligionList { get; set; }
		public List<Subcategory> PreviousPersonalityList { get; set; }
		public List<Subcategory> PreviousQualificationTypeList { get; set; }
		public List<Subcategory> PreviousNursingQualificationsList { get; set; }
		public List<Subcategory> PreviousPersonalCareQualificationsList { get; set; }
		public List<Subcategory> PreviousOtherQualificationsList { get; set; }
		public List<Subcategory> PreviousVerificationChecksList { get; set; }
		public List<Subcategory> PreviousOtherPreferenceList { get; set; }
		public List<Subcategory> PreviousDisabilityCareExperienceList { get; set; }
		public List<Subcategory> PreviousMentalHealthCareExperienceList { get; set; }
		public List<Subcategory> PreviousAgedCareExperienceList { get; set; }
		public List<Subcategory> PreviousChronicMedicalConditionsCareExperienceList { get; set; }
		public List<Subcategory> PreviousInterestList { get; set; }
	}
}