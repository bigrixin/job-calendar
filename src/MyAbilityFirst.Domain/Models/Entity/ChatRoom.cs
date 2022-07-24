using System;
using System.Collections.Generic;
using System.Linq;

namespace MyAbilityFirst.Domain
{
	public class ChatRoom
	{
		public int ID { get; set; }
		public virtual ICollection<Message> Messages { get; set; }
		public virtual ICollection<ChatRoomUser> ChatRoomUsers { get; set; }
		public int LastMessageID { get; set; }
		public string RoomName { get; set; }

		public ChatRoomType ChatRoomType { get; set; }

		protected ChatRoom()
		{
			this.Messages = new List<Message>();
			this.ChatRoomUsers = new List<ChatRoomUser>();
		}

		public ChatRoom(ChatRoomType chatRoomType, string roomName)
		{
			this.Messages = new List<Message>();
			this.ChatRoomUsers = new List<ChatRoomUser>();
			this.ChatRoomType = chatRoomType;
			this.RoomName = roomName;
		}

		public bool AddUser(int userID)
		{
			// Room can add user if :
			// (room is 1:1 and user count < 2) or (room is not 1:1)
			// and user doesn't already exist in room
			if (
				( (roomIs1To1() && this.ChatRoomUsers.Count < 2) || (!roomIs1To1()) )
				&& ChatRoomUsers.Where(u => u.OwnerUserID == userID).SingleOrDefault() == null
			)
			{
				this.ChatRoomUsers.Add(new ChatRoomUser(userID, this.ID));
				return true;
			}
			return false;
		}

		public Message AddMessage(string content, int ownerUserID) 
		{
			if (ChatRoomUsers.Where(u => u.OwnerUserID == ownerUserID).SingleOrDefault() != null)
			{
				var message = new Message(content, ownerUserID, this.ID);
				Messages.Add(message);
				return message;
			}
			return null;
		}

		private bool roomIs1To1()
		{
			return (this.ChatRoomType == ChatRoomType.Private || this.ChatRoomType == ChatRoomType.Job);
		}

	}
}
