using System;
using System.ComponentModel.DataAnnotations;

namespace MyAbilityFirst.Domain
{
	public class EmploymentHistory
	{
		public int ID { get; set; }
		public int CareWorkerID { get; set; }
		public string CompanyName { get; set; }
		public string PositionTitle { get; set; }
		[Required]
		[Display(Name = "Start Month")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0: dd/MMM/yyyy}")]
		public DateTime? StartMonth { get; set; }
		[Required]
		[Display(Name = "End Month")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0: dd/MMM/yyyy}")]
		public DateTime? EndMonth { get; set; }
		public string Description { get; set; }
	}
}
