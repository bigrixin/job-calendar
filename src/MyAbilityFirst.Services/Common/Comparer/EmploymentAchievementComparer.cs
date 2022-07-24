using MyAbilityFirst.Domain;
using System.Collections.Generic;

namespace MyAbilityFirst.Services.Common
{
	public class EmploymentAchievementComparer : IEqualityComparer<EmploymentAchievement>
	{
		public bool Equals(EmploymentAchievement c1, EmploymentAchievement c2)
		{
			return c1.ID == c2.ID;
		}

		public int GetHashCode(EmploymentAchievement c)
		{
			return c.ID.GetHashCode();
		}
	}
}