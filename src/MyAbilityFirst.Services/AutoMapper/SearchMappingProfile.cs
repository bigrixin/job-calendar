using AutoMapper;
using MyAbilityFirst.Models;

namespace MyAbilityFirst.Services.Common
{
	public class SearchMappingProfile : Profile
	{

		#region Fields

		private readonly IPresentationService _presentationService;

		public override string ProfileName => "SearchMappingProfile";

		#endregion

		#region Ctor

		public SearchMappingProfile(IPresentationService presentationService)
		{
			this._presentationService = presentationService;
			this.MapSearchCriteria();
		}

		#endregion

		#region Helpers

		private void MapSearchCriteria() 
		{
			CreateMap<SearchViewModel, SearchViewModel>()
				.ForMember(dest => dest.GenderList, opt => opt.MapFrom(src => _presentationService.GetSubCategoryList("Gender")))
					.AfterMap((src, dest) =>
					{
						dest.PreviousGenderList = _presentationService.GetCrossReferenceSubcategoryList(src.PostedGenderIDs, dest.GenderList);
					})
				.ForMember(dest => dest.LanguageList, opt => opt.MapFrom(src => _presentationService.GetSubCategoryList("Language")))
					.AfterMap((src, dest) =>
					{
						dest.PreviousLanguageList = _presentationService.GetCrossReferenceSubcategoryList(src.PostedLanguageIDs, dest.LanguageList);
					})
				.ForMember(dest => dest.CultureList, opt => opt.MapFrom(src => _presentationService.GetSubCategoryList("Culture")))
					.AfterMap((src, dest) =>
					{
						dest.PreviousCultureList = _presentationService.GetCrossReferenceSubcategoryList(src.PostedCultureIDs, dest.CultureList);
					})
				.ForMember(dest => dest.ReligionList, opt => opt.MapFrom(src => _presentationService.GetSubCategoryList("Religion")))
					.AfterMap((src, dest) =>
					{
						dest.PreviousReligionList = _presentationService.GetCrossReferenceSubcategoryList(src.PostedReligionIDs, dest.ReligionList);
					})
				.ForMember(dest => dest.PersonalityList, opt => opt.MapFrom(src => _presentationService.GetSubCategoryList("Personality")))
					.AfterMap((src, dest) =>
					{
						dest.PreviousPersonalityList = _presentationService.GetCrossReferenceSubcategoryList(src.PostedPersonalityIDs, dest.PersonalityList);
					})
				.ForMember(dest => dest.QualificationTypeList, opt => opt.MapFrom(src => _presentationService.GetSubCategoryList("QualificationType")))
					.AfterMap((src, dest) =>
					{
						dest.PreviousQualificationTypeList = _presentationService.GetCrossReferenceSubcategoryList(src.PostedSubcategoryIDs, dest.QualificationTypeList);
					})
				.ForMember(dest => dest.NursingQualificationsList, opt => opt.MapFrom(src => _presentationService.GetSubCategoryList("NursingQualifications")))
					.AfterMap((src, dest) =>
					{
						dest.PreviousNursingQualificationsList = _presentationService.GetCrossReferenceSubcategoryList(src.PostedSubcategoryIDs, dest.NursingQualificationsList);
					})
				.ForMember(dest => dest.PersonalCareQualificationsList, opt => opt.MapFrom(src => _presentationService.GetSubCategoryList("PersonalCareQualifications")))
					.AfterMap((src, dest) =>
					{
						dest.PreviousPersonalCareQualificationsList = _presentationService.GetCrossReferenceSubcategoryList(src.PostedSubcategoryIDs, dest.PersonalCareQualificationsList);
					})
				.ForMember(dest => dest.OtherQualificationsList, opt => opt.MapFrom(src => _presentationService.GetSubCategoryList("OtherQualifications")))
					.AfterMap((src, dest) =>
					{
						dest.PreviousOtherQualificationsList = _presentationService.GetCrossReferenceSubcategoryList(src.PostedSubcategoryIDs, dest.OtherQualificationsList);
					})
				.ForMember(dest => dest.VerificationChecksList, opt => opt.MapFrom(src => _presentationService.GetSubCategoryList("VerificationChecks")))
					.AfterMap((src, dest) =>
					{
						dest.PreviousVerificationChecksList = _presentationService.GetCrossReferenceSubcategoryList(src.PostedSubcategoryIDs, dest.VerificationChecksList);
					})
				.ForMember(dest => dest.OtherPreferenceList, opt => opt.MapFrom(src => _presentationService.GetSubCategoryList("OtherPreference")))
					.AfterMap((src, dest) =>
					{
						dest.PreviousOtherPreferenceList = _presentationService.GetCrossReferenceSubcategoryList(src.PostedSubcategoryIDs, dest.OtherPreferenceList);
					})
				.ForMember(dest => dest.DisabilityCareExperienceList, opt => opt.MapFrom(src => _presentationService.GetSubCategoryList("DisabilityCareExperience")))
					.AfterMap((src, dest) =>
					{
						dest.PreviousDisabilityCareExperienceList = _presentationService.GetCrossReferenceSubcategoryList(src.PostedSubcategoryIDs, dest.DisabilityCareExperienceList);
					})
				.ForMember(dest => dest.MentalHealthCareExperienceList, opt => opt.MapFrom(src => _presentationService.GetSubCategoryList("MentalHealthCareExperience")))
					.AfterMap((src, dest) =>
					{
						dest.PreviousMentalHealthCareExperienceList = _presentationService.GetCrossReferenceSubcategoryList(src.PostedSubcategoryIDs, dest.MentalHealthCareExperienceList);
					})
				.ForMember(dest => dest.AgedCareExperienceList, opt => opt.MapFrom(src => _presentationService.GetSubCategoryList("AgedCareExperience")))
					.AfterMap((src, dest) =>
					{
						dest.PreviousAgedCareExperienceList = _presentationService.GetCrossReferenceSubcategoryList(src.PostedSubcategoryIDs, dest.AgedCareExperienceList);
					})
				.ForMember(dest => dest.ChronicMedicalConditionsCareExperienceList, opt => opt.MapFrom(src => _presentationService.GetSubCategoryList("ChronicMedicalConditionsCareExperience")))
					.AfterMap((src, dest) =>
					{
						dest.PreviousChronicMedicalConditionsCareExperienceList = _presentationService.GetCrossReferenceSubcategoryList(src.PostedSubcategoryIDs, dest.ChronicMedicalConditionsCareExperienceList);
					})
				.ForMember(dest => dest.InterestList, opt => opt.MapFrom(src => _presentationService.GetSubCategoryList("Interest")))
					.AfterMap((src, dest) =>
					{
						dest.PreviousInterestList = _presentationService.GetCrossReferenceSubcategoryList(src.PostedSubcategoryIDs, dest.InterestList);
					})
				.ForMember(dest => dest.UserID, opt => opt.MapFrom(src => src.UserID))
					.AfterMap((src, dest) =>
					{
						var Address = _presentationService.GetUserAddress(dest.UserID);
						dest.HomeLongitude = Address == null ? null : (decimal?) Address.Longitude;
						dest.HomeLatitude = Address == null ? null : (decimal?) Address.Latitude;
						if (Address != null && Address.FullAddress != null) {
							dest.Latitude = src.Latitude == null ? Address.Latitude : src.Latitude;
							dest.Longitude = src.Longitude == null ? Address.Longitude : src.Longitude;
						} else {
							// set default as Brisbane
							dest.Latitude = src.Latitude == null ? (decimal?) -27.469770700 : src.Latitude;
							dest.Longitude = src.Longitude == null ? (decimal?) 153.025123500 : src.Longitude;
						}
					})
				.ForMember(dest => dest.PageNumber, opt => opt.Condition(src => src.PageNumber > 0))
				.ForMember(dest => dest.PageSize, opt => opt.Condition(src => src.PageSize > 0))
				.ForMember(dest => dest.SearchURL, opt => opt.MapFrom(src => "/Search/SearchCareWorkerPaged"))
				.ForAllOtherMembers(opt => opt.Ignore())
				;
		}

		#endregion

	}
}

