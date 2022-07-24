using System;

namespace MyAbilityFirst.Domain
{
	public class Message
	{
		public int ID { get; set; }
		public int ChatRoomID { get; set; }
		public int OwnerUserID { get; set; }
		public string Content { get; set; }
		public DateTime Timestamp { get; set; }

		protected Message()
		{
			// required by EF
		}

		public Message(string content, int ownerUserID, int chatRoomID) 
		{
			ChatRoomID = chatRoomID;
			OwnerUserID = ownerUserID;
			Content = content;
			Timestamp = DateTime.Now;
		}

	}
}
