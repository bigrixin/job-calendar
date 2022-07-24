using MyAbilityFirst.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MyAbilityFirst.Models
{
	public class JobPreviewViewModel
	{
		public int JobId { get; set; }
		public int ClientId { get; set; }

		[Display(Name ="Name")]
		public int PatientName { get; set; }

		public string Gender { get; set; }
		public int Age { get; set; }
		public String Interests { get; set; }

		public string Title { get; set; }

		[Display(Name = "Job Details")]
		public string Description { get; set; }
 
		public string Location { get; set; }

		[Display(Name = "Carer Gender Preference")]
		public string PreferredGender { get; set; }

		[Display(Name = "Start Date")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy}")]
		public DateTime? ServicedAt { get; set; }

		[Display(Name = "Start Time")]
		public DateTime? StartTime { get; set; }
		[Display(Name = "End Time")]
		public DateTime? EndTime { get; set; }

		public IEnumerable<SelectListItem> GenderDropDownList { get; set; }
		public IEnumerable<SelectListItem> ServiceDropDownList { get; set; }
		public IEnumerable<SelectListItem> PatientDropDownList { get; set; }
	}
}