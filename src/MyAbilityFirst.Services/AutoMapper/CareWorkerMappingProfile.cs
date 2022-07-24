using AutoMapper;
using MyAbilityFirst.Domain;
using MyAbilityFirst.Models;
using MyAbilityFirst.Services.ChatRoomFunctions;
using System;
using System.Linq;

namespace MyAbilityFirst.Services.Common
{
	public class CareWorkerMappingProfile : Profile
	{

		#region Fields

		private readonly IPresentationService _presentationService;

		public override string ProfileName => "CareWorkerMappingProfile";

		#endregion

		#region Ctor

		public CareWorkerMappingProfile(IPresentationService presentationService)
		{
			this._presentationService = presentationService;

			this.MapCareWorker();
			this.MapEmploymentDetails();
			this.MapAvailability();
			this.MapReplacement();
		}

		#endregion

		#region Helpers

		private void MapCareWorker()
		{
			CreateMap<CareWorker, CareWorkerDetailsViewModel>()
				.ForMember(dest => dest.GenderDropDownList, opt => opt.MapFrom(src => this._presentationService.GetSubCategorySelectList("Gender")))
					.AfterMap((src, dest) =>
					{
						dest.Gender = dest.GenderDropDownList.SingleOrDefault(list => list.Value == src.GenderID.ToString()) == null ?
						"" : dest.GenderDropDownList.SingleOrDefault(list => list.Value == src.GenderID.ToString()).Text;
					})
					.AfterMap((src, dest) =>
					{
						dest.GenderPreference = dest.GenderDropDownList.SingleOrDefault(list => list.Value == src.GenderPreferenceID.ToString()) == null ?
						"" : dest.GenderDropDownList.SingleOrDefault(list => list.Value == src.GenderPreferenceID.ToString()).Text;
					})
				.ForMember(dest => dest.LanguageDropDownList, opt => opt.MapFrom(src => this._presentationService.GetSubCategorySelectList("Language")))
					.AfterMap((src, dest) =>
					{
						dest.FirstLanguage = dest.LanguageDropDownList.SingleOrDefault(list => list.Value == src.FirstLanguageID.ToString()) == null ?
						"" : dest.LanguageDropDownList.SingleOrDefault(list => list.Value == src.FirstLanguageID.ToString()).Text;
					})
					.AfterMap((src, dest) =>
					{
						dest.SecondLanguage = dest.LanguageDropDownList.SingleOrDefault(list => list.Value == src.SecondLanguageID.ToString()) == null ?
						"" : dest.LanguageDropDownList.SingleOrDefault(list => list.Value == src.SecondLanguageID.ToString()).Text;
					})
				.ForMember(dest => dest.CultureDropDownList, opt => opt.MapFrom(src => this._presentationService.GetSubCategorySelectList("Culture")))
					.AfterMap((src, dest) =>
					{
						dest.Culture = dest.CultureDropDownList.SingleOrDefault(list => list.Value == src.CultureID.ToString()) == null ?
						"" : dest.CultureDropDownList.SingleOrDefault(list => list.Value == src.CultureID.ToString()).Text;
					})
				.ForMember(dest => dest.ReligionDropDownList, opt => opt.MapFrom(src => this._presentationService.GetSubCategorySelectList("Religion")))
					.AfterMap((src, dest) =>
					{
						dest.Religion = dest.ReligionDropDownList.SingleOrDefault(list => list.Value == src.ReligionID.ToString()) == null ?
						"" : dest.ReligionDropDownList.SingleOrDefault(list => list.Value == src.ReligionID.ToString()).Text;
					})
				.ForMember(dest => dest.PersonalityDropDownList, opt => opt.MapFrom(src => this._presentationService.GetSubCategorySelectList("Personality")))
					.AfterMap((src, dest) =>
					{
						dest.Personality = dest.PersonalityDropDownList.SingleOrDefault(list => list.Value == src.PersonalityID.ToString()) == null ?
						"" : dest.PersonalityDropDownList.SingleOrDefault(list => list.Value == src.PersonalityID.ToString()).Text;
					})
				.ForMember(dest => dest.FormalEducationDropDownList, opt => opt.MapFrom(src => this._presentationService.GetSubCategorySelectList("FormalEducation")))
				.ForMember(dest => dest.MarketingInfoList, opt => opt.MapFrom(src => this._presentationService.GetSubCategoryList("CareWorkerMarketingInfo")))
				.ForMember(dest => dest.QualificationTypeList, opt => opt.MapFrom(src => this._presentationService.GetSubCategoryList("QualificationType")))
				.ForMember(dest => dest.NursingQualificationsList, opt => opt.MapFrom(src => this._presentationService.GetSubCategoryList("NursingQualifications")))
				.ForMember(dest => dest.PersonalCareQualificationsList, opt => opt.MapFrom(src => this._presentationService.GetSubCategoryList("PersonalCareQualifications")))
				.ForMember(dest => dest.OtherQualificationsList, opt => opt.MapFrom(src => this._presentationService.GetSubCategoryList("OtherQualifications")))
				.ForMember(dest => dest.VerificationChecksList, opt => opt.MapFrom(src => this._presentationService.GetSubCategoryList("VerificationChecks")))
				.ForMember(dest => dest.OtherPreferenceList, opt => opt.MapFrom(src => this._presentationService.GetSubCategoryList("OtherPreference")))
				.ForMember(dest => dest.DisabilityCareExperienceList, opt => opt.MapFrom(src => this._presentationService.GetSubCategoryList("DisabilityCareExperience")))
				.ForMember(dest => dest.MentalHealthCareExperienceList, opt => opt.MapFrom(src => this._presentationService.GetSubCategoryList("MentalHealthCareExperience")))
				.ForMember(dest => dest.AgedCareExperienceList, opt => opt.MapFrom(src => this._presentationService.GetSubCategoryList("AgedCareExperience")))
				.ForMember(dest => dest.ChronicMedicalConditionsCareExperienceList, opt => opt.MapFrom(src => this._presentationService.GetSubCategoryList("ChronicMedicalConditionsCareExperience")))
				.ForMember(dest => dest.InterestList, opt => opt.MapFrom(src => this._presentationService.GetSubCategoryList("Interest")))
				.ForMember(dest => dest.DayOfWeekList, opt => opt.MapFrom(src => this._presentationService.GetDayOfWeekList()))
				.ForMember(dest => dest.TimeOfDayList, opt => opt.MapFrom(src => this._presentationService.GetTimeOfDayList()))
				.ForMember(dest => dest.PreviousMarketingInfoList, opt => opt.MapFrom(src => this._presentationService.GetSubCategoryListByUser("CareWorkerMarketingInfo", src.ID)))
				.ForMember(dest => dest.PreviousQualificationTypeList, opt => opt.MapFrom(src => this._presentationService.GetSubCategoryListByUser("QualificationType", src.ID)))
				.ForMember(dest => dest.PreviousNursingQualificationsList, opt => opt.MapFrom(src => this._presentationService.GetSubCategoryListByUser("NursingQualifications", src.ID)))
				.ForMember(dest => dest.PreviousPersonalCareQualificationsList, opt => opt.MapFrom(src => this._presentationService.GetSubCategoryListByUser("PersonalCareQualifications", src.ID)))
				.ForMember(dest => dest.PreviousOtherQualificationsList, opt => opt.MapFrom(src => this._presentationService.GetSubCategoryListByUser("OtherQualifications", src.ID)))
				.ForMember(dest => dest.PreviousVerificationChecksList, opt => opt.MapFrom(src => this._presentationService.GetSubCategoryListByUser("VerificationChecks", src.ID)))
				.ForMember(dest => dest.PreviousOtherPreferenceList, opt => opt.MapFrom(src => this._presentationService.GetSubCategoryListByUser("OtherPreference", src.ID)))
				.ForMember(dest => dest.PreviousDisabilityCareExperienceList, opt => opt.MapFrom(src => this._presentationService.GetSubCategoryListByUser("DisabilityCareExperience", src.ID)))
				.ForMember(dest => dest.PreviousMentalHealthCareExperienceList, opt => opt.MapFrom(src => this._presentationService.GetSubCategoryListByUser("MentalHealthCareExperience", src.ID)))
				.ForMember(dest => dest.PreviousAgedCareExperienceList, opt => opt.MapFrom(src => this._presentationService.GetSubCategoryListByUser("AgedCareExperience", src.ID)))
				.ForMember(dest => dest.PreviousChronicMedicalConditionsCareExperienceList, opt => opt.MapFrom(src => this._presentationService.GetSubCategoryListByUser("ChronicMedicalConditionsCareExperience", src.ID)))
				.ForMember(dest => dest.PreviousInterestList, opt => opt.MapFrom(src => this._presentationService.GetSubCategoryListByUser("Interest", src.ID)))
				.ForMember(dest => dest.EmploymentHistories, opt => opt.MapFrom(src => src.EmploymentHistories))
				.ForMember(dest => dest.EmploymentFormalEducations, opt => opt.MapFrom(src => src.EmploymentFormalEducations))
				.ForMember(dest => dest.EmploymentReferences, opt => opt.MapFrom(src => src.EmploymentReferences))
				.ForMember(dest => dest.EmploymentAchievements, opt => opt.MapFrom(src => src.EmploymentAchievements))
				.ForMember(dest => dest.Availabilities, opt => opt.MapFrom(src => this._presentationService.GetPrefilledAvailabilityList(src.ID, src.Availabilities)))
				.ForMember(dest => dest.OrganisationDropDownList, opt => opt.MapFrom(src => this._presentationService.GetOrganisationList()));

			CreateMap<CareWorkerDetailsViewModel, CareWorker>()
				.ForMember(dest => dest.EmploymentHistories, opt => opt.Ignore())
				.ForMember(dest => dest.EmploymentFormalEducations, opt => opt.Ignore())
				.ForMember(dest => dest.EmploymentReferences, opt => opt.Ignore())
				.ForMember(dest => dest.EmploymentAchievements, opt => opt.Ignore())
				.ForMember(dest => dest.Availabilities, opt => opt.Ignore());

			CreateMap<CareWorker, CareWorkerPublicProfileViewModel>()
				.ForMember(dest => dest.Culture, opt => opt.MapFrom(src => this._presentationService.GetSubCategoryName(src.CultureID)))
				.ForMember(dest => dest.Personality, opt => opt.MapFrom(src => this._presentationService.GetSubCategoryName(src.PersonalityID)))
				.ForMember(dest => dest.HomeLatitude, opt => opt.MapFrom(src => src.Address.Latitude))
				.ForMember(dest => dest.HomeLongitude, opt => opt.MapFrom(src => src.Address.Longitude))
				.ForMember(dest => dest.OrganisationDropDownList, opt => opt.MapFrom(src => this._presentationService.GetOrganisationList()))
			 	.ForMember(dest => dest.OrganisationLogoURL, opt => opt.MapFrom(src => this._presentationService.GetOrganisationLogoURL(src.OrganisationId)))
				.ForMember(dest => dest.OverallRating, opt => opt.MapFrom(src => Math.Round(src.TotalRating / src.RatingCount, 1)))
				.ForMember(dest => dest.CareWorkerID, opt => opt.MapFrom(src => src.ID))
				.ForMember(dest => dest.MessageRecipientID, opt => opt.MapFrom(src => src.ID));
		}

		private void MapEmploymentDetails()
		{
			CreateMap<EmploymentHistory, EmploymentHistory>();

			CreateMap<EmploymentFormalEducation, EmploymentFormalEducation>();

			CreateMap<EmploymentReference, EmploymentReference>();

			CreateMap<EmploymentAchievement, EmploymentAchievement>();
		}

		private void MapAvailability()
		{
			CreateMap<Availability, Availability>();
		}

		private void MapReplacement()
		{
			CreateMap<Replacement, ReplacementViewModel>()
			 .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
		   .ForMember(dest => dest.CareWorkerID, opt => opt.MapFrom(src => src.CareWorkerID))
			 .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
			 .ForMember(dest => dest.CommentByCareWorker, opt => opt.MapFrom(src => src.CommentByCareWorker));

			CreateMap<ReplacementViewModel, Replacement>()
			 .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
			 .ForMember(dest => dest.BookingID, opt => opt.MapFrom(src => src.BookingID))
			 .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
			 .ForMember(dest => dest.ReplacedCareWorkerID, opt => opt.MapFrom(src => src.ReplacedCareWorkerID))
			 .ForMember(dest => dest.CommentByCoordinator, opt => opt.MapFrom(src => src.CommentByCoordinator));
		}
		#endregion

	}
}
