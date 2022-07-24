using System;
using System.Collections.Generic;

namespace MyAbilityFirst.Domain
{
	public class CaseNote
	{

		#region Properties

		public int ID { get; set; }
		public int BookingID { get; private set; }
		public int OwnerUserID { get; private set; }
		public string Note { get; set; }
		public DateTime CreatedAt { get; private set; }

		#endregion

		#region Ctor

		protected CaseNote()
		{
			// Required by EF
		}

		public CaseNote(int bookingID, int userId, string note)
		{
			this.BookingID = BookingID;
			this.OwnerUserID = userId;
			this.Note = note;

			this.CreatedAt = DateTime.Now;
		}

		#endregion

	}
}