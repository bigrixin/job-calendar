using MyAbilityFirst.Domain;
using System.Collections.Generic;

namespace MyAbilityFirst.Services.Common
{
	public class EmploymentHistoryComparer : IEqualityComparer<EmploymentHistory>
	{
		public bool Equals(EmploymentHistory c1, EmploymentHistory c2)
		{
			return c1.ID == c2.ID;
		}

		public int GetHashCode(EmploymentHistory c)
		{
			return c.ID.GetHashCode();
		}
	}
}
