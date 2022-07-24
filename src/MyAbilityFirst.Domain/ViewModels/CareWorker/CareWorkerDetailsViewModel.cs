using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Web.Mvc;
using MyAbilityFirst.Domain;

namespace MyAbilityFirst.Models
{
	public class CareWorkerDetailsViewModel : UserDetailsViewModel
	{

		// CareWorker
		public int GenderPreferenceID { get; set; }

		public int LocationCoverageRadius { get; set; }
		public int FirstLanguageID { get; set; }
		public int SecondLanguageID { get; set; }
		public int CultureID { get; set; }
		public int ReligionID { get; set; }
		public int PersonalityID { get; set; }
		public string Description { get; set; }

		[Display(Name = "Work for Organisation")]
		public int OrganisationID { get; set; }
		[Required]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy}")]
		[Display(Name = "Commencement Date")]
		public DateTime? StartingDate { get; set; }

		public List<EmploymentHistory> EmploymentHistories { get; set; }
		public List<EmploymentFormalEducation> EmploymentFormalEducations { get; set; }
		public List<EmploymentReference> EmploymentReferences { get; set; }
		public List<EmploymentAchievement> EmploymentAchievements { get; set; }
		public List<Availability> Availabilities { get; set; }

		// View Texts
		public string Gender { get; set; }
		public string GenderPreference { get; set; }
		public string Suburb { get; set; }
		public string FirstLanguage { get; set; }
		public string SecondLanguage { get; set; }
		public string Culture { get; set; }
		public string Religion { get; set; }
		public string Personality { get; set; }

		// single select dropdowns
		public SelectList GenderDropDownList { get; set; }
		public SelectList LanguageDropDownList { get; set; }
		public SelectList CultureDropDownList { get; set; }
		public SelectList ReligionDropDownList { get; set; }
		public SelectList PersonalityDropDownList { get; set; }
		public SelectList FormalEducationDropDownList { get; set; }
		public SelectList OrganisationDropDownList { get; set; }

		// multi selects
		public List<Subcategory> MarketingInfoList { get; set; }
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
		public List<DayOfWeek> DayOfWeekList { get; set; }
		public List<TimeOfDay> TimeOfDayList { get; set; }

		// for tracking changes
		public List<Subcategory> PreviousMarketingInfoList { get; set; }
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
		public int[] PostedSubCategoryIDs { get; set; }

	}
}
