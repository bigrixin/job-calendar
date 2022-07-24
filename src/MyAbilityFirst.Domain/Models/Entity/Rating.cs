using System;

namespace MyAbilityFirst.Domain
{

	public class Rating
	{

		#region Properties

		public int ID { get; set; }
		public int BookingID { get; set; }
		public int OverallScore { get; set; }
		public string Comments { get; set; }
		public int? PunctualityScore { get; set; }
		public int? CommunicationScore { get; set; }
		public int? ServiceQualityScore { get; set; }
		public int? SatisfactionCheckingScore { get; set; }
		public int? ResponsivenessScore { get; set; }
		public int? FollowUpScore { get; set; }
		public int? FutureRecommendationScore { get; set; }
		public RatingStatus Status { get; set; }
		public int? CoordinatorID { get; set; }
		public DateTime? ApprovedDate { get; set; }

		#endregion

		#region Ctor

		protected Rating()
		{
			// Required by EF
		}

		public Rating(int bookingID, int score, string comments)
		{
			this.BookingID = bookingID;
			this.OverallScore = score;
			this.Comments = comments;
		}

		public Rating(int bookingID)
		{
			this.BookingID = bookingID;
		}

		#endregion

	}

}
