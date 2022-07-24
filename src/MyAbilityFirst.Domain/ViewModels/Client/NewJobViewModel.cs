using MyAbilityFirst.Domain;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MyAbilityFirst.Models
{
	public class NewJobViewModel : NewJobScheduleViewModel
	{
		public int ID { get; set; }
		[Required]
		[Display(Name = "Select a Patient *")]
		public int PatientID { get; set; }
		public int ClientID { get; set; }

		[Required(AllowEmptyStrings = false)]
		[Display(Name = "Job Title *")]
		public string Title { get; set; }

		[Display(Name = "Carer Gender Preference")]
		public int PreferredGenderID { get; set; }

		[Required]
		[Display(Name = "Job Description *")]
		public string Description { get; set; }

		[Display(Name = "Address where care is required *")]
		public Address Address { get; set; }

		[Display(Name = "Same as Patient profile")]
		public bool AddressSameProfile { get; set; }

		[Required(ErrorMessage = "Please select at least one option")]
		public List<int> PatientIDs { get; set; }
		[Display(Name = "Select a Patient *")]
		public List<bool> SelectedPatients { get; set; }
		public JobStatus JobStatus { get; set; }
		public IEnumerable<SelectListItem> GenderDropDownList { get; set; }
		public IEnumerable<SelectListItem> PatientDropDownList { get; set; }

	}
}