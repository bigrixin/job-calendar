using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MyAbilityFirst.Domain.ClientFunctions
{
	public class NewBookingViewModel
	{
		public IEnumerable<SelectListItem> Shortlist;

		[DisplayName("Whom are you requesting a service?")]
		[Required(ErrorMessage = "Please select a care worker")]
		public int CareWorkerID { get; set; }
		public int ClientID { get; set; }
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy hh:mm tt}")]
		[Required]
		public DateTime Start { get; set; }

		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy hh:mm tt}")]
		[Required]
		public DateTime End { get; set; }

		[DisplayName("(Optional) Add a personal note")]
		public string Message { get; set; }
	}
}