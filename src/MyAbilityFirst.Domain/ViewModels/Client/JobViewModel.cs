using MyAbilityFirst.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MyAbilityFirst.Models
{
	public class JobViewModel
	{
		[Key]
		public int Id { get; set; }
		[Display(Name ="Patient")]
		public int PatientId { get; set; }
		public int ClientId { get; set; }
		[Required(AllowEmptyStrings = false)]
		[Display(Name = "Title")]
		public string Title { get; set; }

		[Required]
		[Display(Name = "Description")]
		public string Description { get; set; }
 
		public Address Address { get; set; }

		[Display(Name = "Gender")]
		public int GenderId { get; set; }

		[Display(Name = "Service Required")]
		public int ServiceId { get; set; }
 
		[Display(Name = "Picture")]
		public string PictureURL { get; set; }

		[Required]
		[Display(Name = "Start Date")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MMM/yyyy}")]
		public DateTime? ServicedAt { get; set; }

		public DateTime? CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }

		public IEnumerable<SelectListItem> GenderDropDownList { get; set; }
		public IEnumerable<SelectListItem> ServiceDropDownList { get; set; }
		public IEnumerable<SelectListItem> PatientDropDownList { get; set; }
	}
}