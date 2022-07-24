using System;

namespace MyAbilityFirst.Domain
{
	public abstract class Notification
	{

		#region Properties

		public int ID { get; set; }
		public int OwnerUserID { get; private set; }
		public DateTime NotifiedDate { get; protected set; }
		public DateTime? ReadDate { get; protected set; }
		public string NoticeFrom { get; set; }
		public bool Closed { get; set; }

		#endregion

		#region Ctor

		protected Notification()
		{
			// required by EF
		}

		public Notification(int ownerUserID, string noticeFrom)
		{
			OwnerUserID = ownerUserID;
			NoticeFrom = noticeFrom;
			NotifiedDate = DateTime.Now;
		}

		public virtual bool Read()
		{
			return ReadDate != null;
		}

		public virtual void SetAsRead()
		{
			this.ReadDate = DateTime.Now;
		} 
		#endregion

	}
}
