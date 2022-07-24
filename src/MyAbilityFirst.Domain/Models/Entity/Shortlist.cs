using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAbilityFirst.Domain
{
	public class Shortlist
	{
		#region Properties

		public int ID { get; set; }
		public int OwnerUserID { get; set; }
		public int SelectedUserID { get; set; }
		public bool Selected { get; set; }

		#endregion

		#region Ctor

		protected Shortlist()
		{
			// Required by EF
		}

		public Shortlist(int ownerUserID, int selectedUserID, bool selected)
		{
			this.OwnerUserID = ownerUserID;
			this.SelectedUserID = selectedUserID;
			this.Selected = selected;
		}

		#endregion
	}
}
