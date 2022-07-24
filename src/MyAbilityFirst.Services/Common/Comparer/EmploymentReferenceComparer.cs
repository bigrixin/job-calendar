using MyAbilityFirst.Domain;
using System.Collections.Generic;

namespace MyAbilityFirst.Services.Common
{
	public class EmploymentReferenceComparer : IEqualityComparer<EmploymentReference>
	{
		public bool Equals(EmploymentReference c1, EmploymentReference c2)
		{
			return c1.ID == c2.ID;
		}

		public int GetHashCode(EmploymentReference c)
		{
			return c.ID.GetHashCode();
		}
	}
}
