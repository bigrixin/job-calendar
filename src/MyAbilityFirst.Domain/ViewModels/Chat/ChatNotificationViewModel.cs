namespace MyAbilityFirst.Models
{
	public class ChatNotificationViewModel
	{
		public int ID { get; set; }
		public string NoticeFrom { get; set; }
		public int OwnerUserID { get; set; }
		public int RoomID { get; set; }
		public int NewMessageCount { get; set; }
		public bool Read { get; set; }
		public bool Closed { get; set; }
	}
}
