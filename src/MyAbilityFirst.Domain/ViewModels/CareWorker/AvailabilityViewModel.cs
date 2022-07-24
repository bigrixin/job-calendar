using System;
using MyAbilityFirst.Domain;
using System.Collections.Generic;

namespace MyAbilityFirst.Models
{
	public class AvailabilityViewModel
	{
		public int ID { get; set; }
		public int CareWorkerID { get; set; }
		public DayOfWeek DayOfWeek { get; set; }
		public TimeOfDay TimeOfDay { get; set; }
		public bool Selected { get; set; }
	}
}
