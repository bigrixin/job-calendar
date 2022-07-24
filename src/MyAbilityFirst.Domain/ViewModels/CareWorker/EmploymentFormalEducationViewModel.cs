using System.Web.Mvc;

namespace MyAbilityFirst.Models
{
	public class EmploymentFormalEducationViewModel
	{
		public int ID { get; set; }
		public int CareWorkerID { get; set; }
		public int FormalEducationID { get; set; }
		public string Description { get; set; }
		public string FormalEducation { get; set; }
		public SelectList FormalEducationDropDownList { get; set; }

	}
}
