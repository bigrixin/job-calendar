using System;

namespace MyAbilityFirst.Models
{
	public class MessageViewModel
	{
		public string Message { get; set; }
		public DateTime Timestamp { get; set; }
		public int OwnerUserID { get; set; }
		public string SenderUserName { get; set; }
		public string SenderPictureURL { get; set; }
		public bool FormatAsSenderMessage { get; set; }
	}
}
