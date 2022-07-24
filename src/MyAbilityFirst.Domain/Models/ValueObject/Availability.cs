using System;

namespace MyAbilityFirst.Domain
{
	public class Availability
	{
		public int ID { get; set; }
		public int CareWorkerID { get; set; }
		public DayOfWeek DayOfWeek { get; set; }
		public TimeOfDay TimeOfDay { get; set; }
		public bool Selected { get; set; }
	}
}
