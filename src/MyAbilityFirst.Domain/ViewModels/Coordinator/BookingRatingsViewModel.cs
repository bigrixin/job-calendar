using System;

namespace MyAbilityFirst.Models
{
	public class BookingRatingsViewModel
	{
		public int BookingID { get; set; }
		public int ClientID { get; set; }
		public int CareWorkerID { get; set; }
		public int RatingID { get; set; }
		public string ClientFirstName { get; set; }
		public string CareWorkerFirstName { get; set; }
		public double OverallScore { get; set; }
		public DateTime CreatedAt { get; set; }
		public bool Approved { get; set; }
	}
}
