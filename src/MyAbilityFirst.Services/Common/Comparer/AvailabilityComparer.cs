using MyAbilityFirst.Domain;
using System.Collections.Generic;

namespace MyAbilityFirst.Services.Common
{
	public class AvailabilityComparer : IEqualityComparer<Availability>
	{
		public bool Equals(Availability c1, Availability c2)
		{
			return c1.ID == c2.ID;
		}

		public int GetHashCode(Availability c)
		{
			return c.ID.GetHashCode();
		}
	}
}