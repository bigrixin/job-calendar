using System;

namespace MyAbilityFirst.Domain
{
	public class ChatNotification : Notification
	{
		#region Properties

		public int RoomID { get; private set; }
		public int NewMessageCount { get; protected set; }

		#endregion

		protected ChatNotification()
		{
			// required by EF
		}

		public ChatNotification(int ownerUserID, string content, int roomID) : base(ownerUserID, content)
		{
			RoomID = roomID;
			NewMessageCount = 1;
		}

		public void AddNewMessageCount()
		{
			NewMessageCount++;
			this.ReadDate = null;
			this.Closed = false;
			this.NotifiedDate = DateTime.Now;
		}

		public override void SetAsRead()
		{
			base.SetAsRead();
			NewMessageCount = 0;
		}

	}
}
