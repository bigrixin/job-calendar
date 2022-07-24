using MyAbilityFirst.Domain;
using System.Collections.Generic;
using System;

namespace MyAbilityFirst.Models
{
	public class JobsViewModel
	{
		public int ID { get; set; }
		public string Patients { get; set; }
		public int ClientID { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public string Schedule { get; set; }
		public Address Address { get; set; }
		public JobStatus JobStatus { get; set; }
		public DateTime? CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public string SelectedJobsID { get; set; }
		public List<bool> SelectedJobs { get; set; }
		public DateTime EndDate { get; set; }
		public int Views { get; set; }
	}
}