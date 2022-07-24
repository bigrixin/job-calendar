using MyAbilityFirst.Domain;
using MyAbilityFirst.Models;
using PagedList;
using System.Collections.Generic;

namespace MyAbilityFirst.Services.ChatRoomFunctions
{
	public interface IChatRoomService
	{
		// Chatroom functions
		ChatRoom CreateChatRoom(List<int> userIDs, ChatRoomType roomType, string roomName);
		ChatRoom RetrieveChatRoom(int roomID, int pageNumber = 1, int pageSize = PageSize.DefaultMessagePageSize);
		ChatRoom RetrievePrivateChatRoom(int recipientID, int requesterID, int pageNumber = 1, int pageSize = PageSize.DefaultMessagePageSize);
		bool RoomExists(int roomID);
		bool UserHasPostingPermission(int roomID, int userID);
		List<ChatRoomUser> RetrieveChatRoomUsers(int roomID);

		// Messaging functions
		PagedList<Message> RetrieveMessages(int roomID, int pageNumber = 1, int pageSize = PageSize.DefaultMessagePageSize);
		Message CreateMessage(int roomID, string content, int senderUserID);
		void SendMessage(int roomID, string messageHtmlContent, int senderUserID);

		// Chat notification functions
		List<ChatNotification> CreateOrUpdateChatNotifications(int roomID, string noticeFrom, int senderUserID);
		void SendChatNotification(int ownerUserID, string notificationHtmlContent, int notificationID);
		void SendStartTypingNotification(int roomID, int typingUserID);
		void SendStopTypingNotification(int roomID, int typingUserID);
	}
}
