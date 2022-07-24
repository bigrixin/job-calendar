using MyAbilityFirst.Domain;
using System.Collections.Generic;

namespace MyAbilityFirst.Services.Common
{
	public class UserComparer<T> : IEqualityComparer<T> where T : User
	{
		public bool Equals(T u1, T u2)
		{
			return u1.ID == u2.ID;
		}

		public int GetHashCode(T u)
		{
			return u.ID.GetHashCode();
		}
	}
}