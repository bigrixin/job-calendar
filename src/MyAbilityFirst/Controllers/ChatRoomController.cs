using AutoMapper;
using MyAbilityFirst.Domain;
using MyAbilityFirst.Services.ChatRoomFunctions;
using MyAbilityFirst.Models;
using System.Web.Mvc;
using System.Collections.Generic;
using MyAbilityFirst.Services.Common;

namespace MyAbilityFirst.Controllers
{
	[Authorize]
	public class ChatRoomController : Controller
	{

		#region Fields

		private readonly IMapper _mapper;
		private readonly IChatRoomService _chatRoomServices;
		private readonly IUserService _userServices;

		#endregion

		#region Ctor

		public ChatRoomController(IMapper mapper, IChatRoomService chatRoomServices, IUserService userServices)
		{
			this._mapper = mapper;
			this._chatRoomServices = chatRoomServices;
			this._userServices = userServices;
		}

		#endregion

		#region Actions

		[HttpGet]
		public JsonResult GetMessages(int roomID, int pageNumber = 1, int pageSize = PageSize.DefaultMessagePageSize)
		{
			ChatRoom chatRoom = _chatRoomServices.RetrieveChatRoom(roomID, pageNumber, pageSize);
			ChatRoomViewModel vm = mapChatRoomToVM(chatRoom, pageNumber, pageSize); 
			JsonResult res = getJsonResult("", vm.Messages != null, "_Messages", vm);
			return setResultsAsAllowGet(ref res);
		}

		[HttpPost]
		public JsonResult PostMessage(int roomID, string content = "")
		{
			MessageViewModel vm = new MessageViewModel();
			if (_chatRoomServices.RoomExists(roomID) && content.Length > 0)
			{
				var poster = this.GetLoggedInUser();
				Message message = _chatRoomServices.CreateMessage(roomID, content, poster.ID);

				if (message != null)
				{
					vm = _mapper.Map(message, vm);
					_chatRoomServices.SendMessage(roomID, this.RenderViewToString("_ReceiverMessage", vm), poster.ID);
					sendChatNotification(roomID, poster);
				}
			}
			JsonResult res = getJsonResult("", vm.Message != null, "_SenderMessage", vm);
			return setResultsAsAllowGet(ref res); ;
		}

		[HttpGet]
		public JsonResult GetChatRoomJson(int roomID, int pageNumber = 1, int pageSize = PageSize.DefaultMessagePageSize)
		{
			ChatRoom chatRoom = _chatRoomServices.RetrieveChatRoom(roomID, pageNumber, pageSize);
			ChatRoomViewModel vm = mapChatRoomToVM(chatRoom, pageNumber, pageSize);
			JsonResult res = getJsonResult("", vm.Messages != null, "_PrivateChatRoom", vm);
			return setResultsAsAllowGet(ref res);
		}

		[HttpGet]
		public JsonResult GetPrivateChatRoomJson(int recipientID, int pageNumber = 1, int pageSize = PageSize.DefaultMessagePageSize)
		{
			int requestUserID = this.GetLoggedInUser().ID;
			ChatRoom chatRoom = _chatRoomServices.RetrievePrivateChatRoom(requestUserID, recipientID, pageNumber, pageSize);
			if (chatRoom == null)
				createPrivateRoom(requestUserID, recipientID, pageNumber, pageSize);
			var vm = mapChatRoomToVM(chatRoom, pageNumber, pageSize, requestUserID);
			JsonResult res = getJsonResult("", vm.Messages != null, "_PrivateChatRoom", vm);
			return setResultsAsAllowGet(ref res);
		}
		
		[HttpGet]
		public JsonResult StartTypingNotification(int roomID)
		{
			_chatRoomServices.SendStartTypingNotification(roomID, this.GetLoggedInUser().ID);
			JsonResult res = getJsonResult("", true, "", null);
			return setResultsAsAllowGet(ref res);
		}

		[HttpGet]
		public JsonResult StopTypingNotification(int roomID)
		{
			_chatRoomServices.SendStopTypingNotification(roomID, this.GetLoggedInUser().ID);
			JsonResult res = getJsonResult("", true, "", null);
			return setResultsAsAllowGet(ref res);
		}

		[HttpPost]
		public JsonResult ChatNotificationRead(int notificationID)
		{
			var ownerUserID = this.GetLoggedInUser().ID;
			return setChatNotificationAsRead(_userServices.RetrieveNotification<ChatNotification>(notificationID), ownerUserID);
		}

		[HttpPost]
		public JsonResult ChatNotificationReadViaRoom(int roomID)
		{
			var ownerUserID = this.GetLoggedInUser().ID;
			return setChatNotificationAsRead(_userServices.RetrieveChatNotification(ownerUserID, roomID), ownerUserID);
		}

		[HttpPost]
		public JsonResult ChatNotificationClosed(int notificationID)
		{
			var ownerUserID = this.GetLoggedInUser().ID;
			ChatNotification chatNotification = _userServices.RetrieveNotification<ChatNotification>(notificationID);
			if (chatNotification != null)
			{
				chatNotification = _userServices.SetNotificationAsClosed(chatNotification);
			}
			JsonResult res = getJsonResult(chatNotification != null);
			return setResultsAsAllowGet(ref res);
		}

		#endregion

		#region Helpers

		private ChatRoomViewModel mapChatRoomToVM(ChatRoom chatRoom, int pageNumber, int pageSize, int? requestUserID = null)
		{
			ChatRoomViewModel vm = new ChatRoomViewModel();
			if (chatRoom != null)
			{
				requestUserID = requestUserID ?? this.GetLoggedInUser().ID;
				vm = _mapper.Map(chatRoom, vm, opt => opt.BeforeMap((src, dest) => {
					dest.RequestedUserID = (int)requestUserID;
					dest.PageNumber = pageNumber;
					dest.PageSize = pageSize;
				}));
			}
			return vm;
		}

		private JsonResult getJsonResult(string html, bool success, string partialViewName, object vm)
		{
			if (success && html.Length == 0 && partialViewName.Length > 0 && vm != null)
				html = this.RenderViewToString(partialViewName, vm);

			return
				Json(new
				{
					Html = html,
					Success = success
				});
		}

		private JsonResult getJsonResult(bool success)
		{
			return
				Json(new
				{
					Success = success
				});
		}

		private JsonResult setResultsAsAllowGet(ref JsonResult result)
		{
			result.ContentEncoding = System.Text.Encoding.UTF8;
			result.ContentType = "application/json";
			result.MaxJsonLength = 100000;
			result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
			result.RecursionLimit = 10;
			return result;
		}

		private ChatRoom createPrivateRoom(int requestUserID, int recipientID, int pageNumber = 1, int pageSize = PageSize.DefaultMessagePageSize)
		{
			List<int> userIDs = new List<int>();
			userIDs.Add(requestUserID);
			userIDs.Add(recipientID);
			var chatRoom = _chatRoomServices.CreateChatRoom(userIDs, ChatRoomType.Private, "");
			return chatRoom;
		}

		private void sendChatNotification(int roomID, User poster)
		{
			// Send notification to recipient about new message
			ChatNotificationViewModel vm = new ChatNotificationViewModel();
			var notification = _chatRoomServices.CreateOrUpdateChatNotifications(roomID, poster.FirstName + " " + poster.LastName, poster.ID)[0];
			vm = _mapper.Map(notification, vm);
			_chatRoomServices.SendChatNotification(notification.OwnerUserID, this.RenderViewToString("_ChatNotification", vm), notification.ID);
		}

		private JsonResult setChatNotificationAsRead(ChatNotification chatNotification, int ownerUserID) 
		{
			ChatNotificationViewModel vm = new ChatNotificationViewModel();
			if (chatNotification != null)
			{
				chatNotification = _userServices.SetNotificationAsRead(chatNotification);
				vm = _mapper.Map(chatNotification, vm);
			}
			JsonResult res = getJsonResult("", chatNotification != null, "_ChatNotification", vm);
			return setResultsAsAllowGet(ref res);
		}

		#endregion

	}
}