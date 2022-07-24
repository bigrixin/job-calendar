using MyAbilityFirst.Domain;
using System.Collections.Generic;

namespace MyAbilityFirst.Services.Common
{
	public class EmploymentFormalEducationComparer : IEqualityComparer<EmploymentFormalEducation>
	{
		public bool Equals(EmploymentFormalEducation c1, EmploymentFormalEducation c2)
		{
			return c1.ID == c2.ID;
		}

		public int GetHashCode(EmploymentFormalEducation c)
		{
			return c.ID.GetHashCode();
		}
	}
}
