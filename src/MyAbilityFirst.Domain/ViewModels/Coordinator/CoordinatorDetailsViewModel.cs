using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MyAbilityFirst.Models
{
	public class CoordinatorDetailsViewModel : UserDetailsViewModel
	{
		public int ID { get; set; }
		public string Gender { get; set; }
		[Display(Name = "Work for Organisation")]
		public int OrganisationID { get; set; }
		[Required]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy}")]
		[Display(Name = "Commencement Date")]
		public DateTime? StartingDate { get; set; }

		public SelectList GenderDropDownList { get; set; }
		public SelectList OrganisationDropDownList { get; set; }
	}
}
