using MyAbilityFirst.Domain;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
	
namespace MyAbilityFirst.Models
{
	public class PatientDetailsViewModel
	{
		public int PatientID { get; set; }
		public int ClientID { get; set; }
        
		// User
		[Required]
		public string FirstName { get; set; }
		[Required]
		public string LastName { get; set; }
		[Required]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
		public DateTime? DoB { get; set; }
		public int GenderID { get; set; }
		[Required]
		[EmailAddress]
		public string Email { get; set; }
		public string Phone { get; set; }
		public Address Address { get; set; }

		// Patient
		public string Allergies { get; set; }

		public List<Contact> Contacts { get; set; }
		
		public string EmergencyResponse { get; set; }
		public HouseDetails HouseDetails { get; set; }
		public MedicationDetails MedicationDetails { get; set; }
		public int FirstLanguageID { get; set; }
		public int SecondLanguageID { get; set; }
		public int CultureID { get; set; }
		public int ReligionID { get; set; }
		public int CareTypeID { get; set; }

		public string GoalsAndAspirations { get; set; }

		// View Texts
		public string Gender { get; set; }
		public string FirstLanguage { get; set; }
		public string SecondLanguage { get; set; }
		public string Culture { get; set; }
		public string Religion { get; set; }
		public string CareType { get; set; }

		// single select dropdowns
		public SelectList GenderDropDownList { get; set; }
		public SelectList CareTypeDropDownList { get; set; }
		public SelectList LanguageDropDownList { get; set; }
		public SelectList CultureDropDownList { get; set; }
		public SelectList ReligionDropDownList { get; set; }
		public SelectList RelationshipDropDownList { get; set; }
		public SelectList PetDropDownList { get; set; }

		// multi selects
		public List<Subcategory> InterestList { get; set; }
		public List<Subcategory> MedicalLivingNeedsList { get; set; }

		// for tracking changes
		public List<Subcategory> PreviousInterestList { get; set; }
		public List<Subcategory> PreviousMedicalLivingNeedsList { get; set; }
		public int[] PostedSubCategoryIDs { get; set; }
	}
}