using MyAbilityFirst.Domain;
using MyAbilityFirst.Models;
using MyAbilityFirst.Infrastructure;
using MyAbilityFirst.Infrastructure.Communication;
using MyAbilityFirst.Services.Common;
using PagedList;
using System.Collections.Generic;
using System.Linq;

namespace MyAbilityFirst.Services.ChatRoomFunctions
{
	public class ChatRoomService : IChatRoomService
	{

		#region Fields

		private readonly IWriteEntities _entities;
		private readonly IBroadcastService _hub;
		private readonly IUserService _userServices;

		#endregion

		#region Ctor

		public ChatRoomService(IWriteEntities entities, IBroadcastService hub, IUserService userServices)
		{
			this._entities = entities;
			this._hub = hub;
			this._userServices = userServices;
		}

		#endregion

		#region Chatroom functions
		
		public ChatRoom CreateChatRoom(List<int> userIDs, ChatRoomType roomType, string roomName)
		{
			return CreateChatRoomFromUserIDs(userIDs, roomType, roomName);
		}

		public ChatRoom RetrieveChatRoom(int roomID, int pageNumber = 1, int pageSize = PageSize.DefaultMessagePageSize)
		{
			return RetrieveDetachedChatRoom(roomID, true, pageNumber, pageSize);
		}

		public ChatRoom RetrievePrivateChatRoom(int recipientID, int requesterID, int pageNumber = 1, int pageSize = PageSize.DefaultMessagePageSize)
		{
			var userIDList = new List<int>();
			userIDList.Add(recipientID);
			userIDList.Add(requesterID);
			return RetrieveDetachedChatRoom(userIDList, ChatRoomType.Private, true, pageNumber, pageSize);
		}

		public bool RoomExists(int roomID)
		{
			return _entities.GetLazyNoTracking<ChatRoom>(r => r.ID == roomID).Any();
		}

		public bool UserHasPostingPermission(int roomID, int userID)
		{	
			// true if user is part of the room. TODO: Permissions setup
			return _entities.GetLazyNoTracking<ChatRoomUser>(u => u.ChatRoomID == roomID && u.OwnerUserID == userID).Any();
		}

		public List<ChatRoomUser> RetrieveChatRoomUsers(int roomID)
		{
			return _entities.GetLazyNoTracking<ChatRoomUser>(u => u.ChatRoomID == roomID).ToList();
		}

		#endregion

		#region Messaging functions

		public PagedList<Message> RetrieveMessages(int roomID, int pageNumber = 1, int pageSize = PageSize.DefaultMessagePageSize)
		{
			var query = this._entities.GetLazy<Message>(m => m.ChatRoomID == roomID);
			int messageCount = query.Count();

			if (pageSize < 1)
				pageSize = PageSize.DefaultMessagePageSize;

			if (pageSize > PageSize.MaxMessagePageSize)
				pageSize = PageSize.MaxMessagePageSize;

			if (pageNumber < 1)
				pageNumber = 1;

			return new PagedList<Message>(query.OrderByDescending(x => x.ID), pageNumber, pageSize);
		}

		public Message CreateMessage(int roomID, string content, int senderUserID)
		{
			ChatRoom room = RetrieveDetachedChatRoom(roomID, true); // Loading room with latest messages
			Message res = room.AddMessage(content, senderUserID); // Create Message via ChatRoom for user verification
			if (res != null)
			{
				this._entities.Create(res); // Save new message directly 
				this._entities.Save();
			}
			return res;
		}

		public void SendMessage(int roomID, string messageHtmlContent, int senderUserID)
		{
			var loginNames = RetrieveChatRoomLoginNamesWithoutSender(roomID, senderUserID);
			_hub.SendMessage(loginNames, messageHtmlContent, roomID);
			
		}

		public List<ChatNotification> CreateOrUpdateChatNotifications(int roomID, string noticeFrom, int senderUserID)
		{
			var users = _entities.Get<ChatRoomUser>(u => u.ChatRoomID == roomID && u.OwnerUserID != senderUserID);
			var res = new List<ChatNotification>();

			foreach (var user in users)
			{
				var notif = _userServices.RetrieveChatNotification(user.OwnerUserID, roomID);

				if (notif != null)
				{
					// update
					notif.AddNewMessageCount();
					_userServices.UpdateNotification(notif);
				} 
				else 
				{
					// create
					notif = new ChatNotification(user.OwnerUserID, noticeFrom, roomID);
					_userServices.CreateNotification(notif);
				}
				res.Add(notif);
			}
			return res;
		}

		#endregion

		#region Chat notifications

		public void SendChatNotification(int ownerUserID, string notificationHtmlContent, int notificationID)
		{
			_userServices.SendNotification(ownerUserID, notificationHtmlContent, notificationID);
		}

		public void SendStartTypingNotification(int roomID, int typingUserID)
		{
			var typingUser = _userServices.GetUser(typingUserID);
			_hub.SendStartTypingNotification(RetrieveChatRoomLoginNamesWithoutSender(roomID, typingUserID), typingUser.FirstName + " " + typingUser.LastName + " is typing...", roomID);
		}

		public void SendStopTypingNotification(int roomID, int typingUserID)
		{
			var typingUser = _userServices.GetUser(typingUserID);
			_hub.SendStopTypingNotification(RetrieveChatRoomLoginNamesWithoutSender(roomID, typingUserID), typingUser.FirstName + " " + typingUser.LastName + " is typing...", roomID);
		}

		#endregion

		#region Helpers

		private ChatRoom CreateChatRoomFromUserIDs(List<int> userIDs, ChatRoomType roomType, string roomName)
		{
			ChatRoom room = new ChatRoom(roomType, roomName);
			foreach (int id in userIDs)
			{
				if (!room.AddUser(id))
					return null;
			}
			this._entities.Create(room);
			this._entities.Save();
			return room;
		}

		private ChatRoom RetrieveDetachedChatRoom(List<int> userIDs, ChatRoomType roomType, bool withPagedMessages, int? pageNumber = 1, int? pageSize = PageSize.DefaultMessagePageSize)
		{
			var messageRoomIDs =
				from u in this._entities.GetLazyNoTracking<ChatRoomUser>()
				join uID in userIDs on u.OwnerUserID equals uID
				group u by u.ChatRoomID into grp
				where grp.Count() == userIDs.Count
				select grp.Key;

			var roomIDs =
				from id in messageRoomIDs
				join r in this._entities.GetLazyNoTracking<ChatRoom>(r => r.ChatRoomType == roomType) on id equals r.ID
				select r.ID;

			return RetrieveDetachedChatRoom(roomIDs.FirstOrDefault(), withPagedMessages, pageNumber, pageSize);
		}

		private ChatRoom RetrieveDetachedChatRoom(int roomID, bool withPagedMessages, int? pageNumber = 1, int? pageSize = PageSize.DefaultMessagePageSize)
		{
			return
				(from r in _entities.GetLazyNoTracking<ChatRoom>(room => room.ID == roomID)
				 select new
				 {
					 ID = r.ID,
					 LastMessageID = r.LastMessageID,
					 ChatRoomType = r.ChatRoomType,
					 ChatRoomUsers = r.ChatRoomUsers,
					 RoomName = r.RoomName
				 }).ToList().Select(r => new ChatRoom(r.ChatRoomType, r.RoomName) { // Returning projection of ChatRoom with paged or without messages
					 ID = r.ID,
					 LastMessageID = r.LastMessageID,
					 ChatRoomType = r.ChatRoomType,
					 ChatRoomUsers = r.ChatRoomUsers,
					 Messages = withPagedMessages ? 
					 _entities.GetLazyNoTracking<Message>(m => m.ChatRoomID == roomID).OrderByDescending(m => m.ID).ToPagedList((int)pageNumber, (int)pageSize).ToList() : null
				}).FirstOrDefault();
		}

		private List<string> RetrieveChatRoomLoginNamesWithoutSender(int roomID, int senderUserID)
		{
			var roomUsers = RetrieveChatRoomUsers(roomID);
			roomUsers.Remove(roomUsers.Single(u => u.OwnerUserID == senderUserID)); // broadcasting can exclude sender
			return roomUsers.Select(
				roomUser => {
					return _userServices.GetUserName(roomUser.OwnerUserID);
				}).ToList();
		}

		#endregion

	}
}

