using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MyAbilityFirst.Models
{
	public class ReplacementViewModel
	{
		public int ID { get; set; }
		[Display(Name = "Booking ID")]
		public int BookingID { get; set; }
		public int ClientID { get; set; }
		[Display(Name = "Client Fistname")]
		public string ClientFirstName { get; set; }
		[Display(Name = "Carer Name")]
		public int CareWorkerID { get; set; }
		[Display(Name = "Carer Fistname")]
		public string CareWorkerFirstName { get; set; }
		[Display(Name = "Comment")]
		public string CommentByCareWorker { get; set; }
		public DateTime? CreatedAt { get; set; }
		[Display(Name = "Replace by Carer")]
		public int? ReplacedCareWorkerID { get; set; }
		public int? CoordinatorID { get; set; }
		[Display(Name = "Comment")]
		public string CommentByCoordinator { get; set; }

		public IEnumerable<SelectListItem> Shortlist;
	}
}
