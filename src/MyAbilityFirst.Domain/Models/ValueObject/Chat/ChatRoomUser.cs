namespace MyAbilityFirst.Domain
{
	public class ChatRoomUser
	{
		public int ID { get; set; }
		public int OwnerUserID { get; set; }
		public int ChatRoomID { get; set; }

		protected ChatRoomUser() 
		{
		}

		public ChatRoomUser(int ownerUserID, int chatRoomID) 
		{
			OwnerUserID = ownerUserID;
			ChatRoomID = chatRoomID;
		}

	}
}
