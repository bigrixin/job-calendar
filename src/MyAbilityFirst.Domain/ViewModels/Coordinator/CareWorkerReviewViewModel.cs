using MyAbilityFirst.Domain;
using System;

namespace MyAbilityFirst.Models
{
	public class CareWorkerReviewViewModel
	{
		public int CareWorkerID { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Suburb { get; set; }
		public string Email { get; set; }
    public string Phone { get; set; }
		public DateTime? CreatedDate { get; set; }
    public UserStatus Status { get; set; }
	}
}
