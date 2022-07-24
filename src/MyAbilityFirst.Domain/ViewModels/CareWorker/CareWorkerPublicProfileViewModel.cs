using MyAbilityFirst.Domain;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MyAbilityFirst.Models
{
	public class CareWorkerPublicProfileViewModel
	{
		public int CareWorkerID { get; set; }
		
		// Chat room
		public int MessageRecipientID { get; set; }
		
		// User
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }


		// CareWorker
		[Display(Name = "Location Coverage Radius")]
		public int LocationCoverageRadius { get; set; }
		public string Description { get; set; }
		public string Gender { get; set; }
		[Display(Name = "Gender Preference")]
		public string GenderPreference { get; set; }
		public string Suburb { get; set; }
		[Display(Name = "Provision Location")]
		public string ProvisionLocation { get; set; }
		[Display(Name = "First Language")]
		public string FirstLanguage { get; set; }
		[Display(Name = "Second Language")]
		public string SecondLanguage { get; set; }
		public string Culture { get; set; }
		public string Personality { get; set; }
		[Display(Name = "Overall Rating")]
		public double OverallRating { get; set; }
		[Display(Name = "Work for Organisation")]
		public int OrganisationID { get; set; }
		[Display(Name = "Logo")]
		public string OrganisationLogoURL { get; set; }
		[Display(Name = "Commencement Date")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy}")]
		public DateTime? StartingDate { get; set; }
		public decimal? HomeLatitude { get; set; }
		public decimal? HomeLongitude { get; set; }

		// Shortlist
		public ShortlistViewModel ShortlistViewModel { get; set; }

		public SelectList OrganisationDropDownList { get; set; }
	}
}