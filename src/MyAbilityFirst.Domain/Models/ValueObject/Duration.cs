using System;

namespace MyAbilityFirst.Domain.Models.ValueObject
{
	public class Duration
	{
		public DateTime Start { get; set; }
		public DateTime End { get; set; }

		public Duration()
		{
		}

		public Duration(DateTime start, DateTime end)
		{
			Start = start;
			End = end;
		}
	}
}
