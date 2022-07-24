using System;
using System.Drawing;

namespace MyAbilityFirst.Models
{
	public class CalendarEventsModel
	{
		public int ID { get; set; }
		public string Title { get; set; }
		public string StartTime { get; set; }
		public string EndTime { get; set; }
		public string Location { get; set; }
		public string Color { get; set; }
	}
}
