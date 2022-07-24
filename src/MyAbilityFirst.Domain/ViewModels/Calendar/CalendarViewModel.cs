using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyAbilityFirst.Models
{
	public class CalendarViewModel
	{
		[Required]
		public IEnumerable<CalendarEventModel> EventGroups { get; set; }
	}
}
