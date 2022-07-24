using System.ComponentModel.DataAnnotations;

namespace MyAbilityFirst.Models
{
	public class RatingViewModel
	{
		public int ID { get; set; }
		public int BookingID { get; set; }
		[Required]
		[Display(Name = "Overall")]
		[Range(1, 5, ErrorMessage = "Rating scale has to be between 1 and 5")]
		public double OverallScore { get; set; }
		public double OldOverallScore { get; set; }
		public string Comments { get; set; }
		[Required]
		[Display(Name = "Did your Care Worker arrive on Time ?")]
		[Range(1, 5, ErrorMessage = "Rating scale has to be between 1 and 5")]
		public double PunctualityScore { get; set; }
		[Required]
		[Display(Name = "Did your Care Worker Communicate to you clearly and openly during the visit ?")]
		[Range(1, 5, ErrorMessage = "Rating scale has to be between 1 and 5")]
		public double CommunicationScore { get; set; }
		[Required]
		[Display(Name = "Did your Care Worker complete the tasks / services to an appropriate standard ?")]
		[Range(1, 5, ErrorMessage = "Rating scale has to be between 1 and 5")]
		public double ServiceQualityScore { get; set; }
		[Required]
		[Display(Name = "Did your Care Worker ask you if you were satisfied before they left ?")]
		[Range(1, 5, ErrorMessage = "Rating scale has to be between 1 and 5")]
		public double SatisfactionCheckingScore { get; set; }
		[Required]
		[Display(Name = "If there was an issue did the Care Worker immediately rectify it ?")]
		[Range(1, 5, ErrorMessage = "Rating scale has to be between 1 and 5")]
		public double ResponsivenessScore { get; set; }
		[Required]
		[Display(Name = "Did the Care Worker contact you post visit to confirm all was ok ?")]
		[Range(1, 5, ErrorMessage = "Rating scale has to be between 1 and 5")]
		public double FollowUpScore { get; set; }
		[Required]
		[Display(Name = "Would you use this Care Worker again ?")]
		[Range(1, 5, ErrorMessage = "Rating scale has to be between 1 and 5")]
		public double FutureRecommendationScore { get; set; }
	}
}
