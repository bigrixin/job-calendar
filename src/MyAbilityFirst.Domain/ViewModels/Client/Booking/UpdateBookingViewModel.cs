using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyAbilityFirst.Domain.ClientFunctions
{
	public class UpdateBookingViewModel : NewBookingViewModel
	{
		[Required]
		public int BookingID { get; set; }
		public bool IsCancelled { get; set; }
		public bool IsLapsed { get; set; }
		public int OwnerUserID { get; set; }
		public string ClientFirstName { get; set; }
		public string CareWorkerFirstName { get; set; }
		public Schedule Schedule { get; set; }
		public BookingStatus Status { get; set; }
		public string Note { get; set; }
		public string Distance { get; set; }

		public int PreviousBookingID { get; set; }
		public List<CaseNote> CaseNotes { get; set; }
		public List<Rating> Rating { get; set; }
	}
}