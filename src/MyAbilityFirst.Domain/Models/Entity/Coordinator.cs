using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAbilityFirst.Domain
{
	public class Coordinator : User
	{
		#region Properties

		public int OrganisationId { get; set; }
		public DateTime? StartingDate { get; set; }

		#endregion

		#region Ctor

		protected Coordinator()
		{
			// required by EF
		}

		public Coordinator(string aspNetIdentity) : base(aspNetIdentity)
		{

		}

		#endregion
	}
}
