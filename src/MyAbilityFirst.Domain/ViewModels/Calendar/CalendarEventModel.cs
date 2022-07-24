using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Google.Apis.Calendar.v3.Data;

namespace MyAbilityFirst.Models
{
	public class CalendarEventModel
	{
		[Required]
		public string GroupTitle { get; set; }

		[Required]
		public IEnumerable<Event> Events { get; set; }
	}
}
