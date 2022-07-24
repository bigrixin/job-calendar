using MyAbilityFirst.Domain;
using System.Collections.Generic;

namespace MyAbilityFirst.Services.Common
{
	public class ContactComparer : IEqualityComparer<Contact>
	{
		public bool Equals (Contact c1, Contact c2)
		{
			return c1.ID == c2.ID;
		}

		public int GetHashCode(Contact c)
		{
			return c.ID.GetHashCode();
		}
	}
}
