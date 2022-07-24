using System.Collections.Generic;

namespace MyAbilityFirst.Models
{
	public class ChatRoomViewModel
	{
		public int RequestedUserID { get; set; }
		public int MessageRecipientID { get; set; }
		public string RoomName { get; set; }
		public int ChatRoomID { get; set; }
		public int PageNumber { get; set; }
		public int PageCount { get; set; }
		public int PageSize { get; set; }
		public string Message { get; set; }
		public List<MessageViewModel> Messages { get; set; }
	}
}
