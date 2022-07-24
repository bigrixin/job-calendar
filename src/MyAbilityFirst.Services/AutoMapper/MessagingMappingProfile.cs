using AutoMapper;
using MyAbilityFirst.Domain;
using MyAbilityFirst.Models;
using System;
using System.Linq;

namespace MyAbilityFirst.Services.Common
{
	public class MessagingMappingProfile : Profile
	{

		#region Fields

		public override string ProfileName => "MessagingMappingProfile";
		private readonly IPresentationService _presentationService;

		#endregion

		#region Ctor

		public MessagingMappingProfile(IPresentationService presentationService)
		{
			this._presentationService = presentationService;
			this.MapMessagesCriteria();
			this.MapChatNotificationCriteria();
		}

		#endregion

		#region Helpers

		private void MapMessagesCriteria()
		{
			CreateMap<ChatRoom, ChatRoomViewModel>()
				.ForMember(dest => dest.Messages, opt => opt.ResolveUsing<MessagesVMResolver>())
				.ForMember(dest => dest.ChatRoomID, opt => opt.MapFrom(src => src.ID))
				.ForMember(dest => dest.RoomName, opt => opt.Ignore())
				.AfterMap((src, dest) => {
					dest.RoomName = src.RoomName.Length == 0 && src.ChatRoomType == ChatRoomType.Private ? // If room is private, Label the room as the targeted chat user
					_presentationService.GetUserFirstLastName(src.ChatRoomUsers.Single(u => u.OwnerUserID != dest.RequestedUserID).OwnerUserID) : src.RoomName;

					dest.MessageRecipientID = src.ChatRoomType == ChatRoomType.Private ? // If room is private, set recipientID for JS to match and not reload room
					src.ChatRoomUsers.Single(u => u.OwnerUserID != dest.RequestedUserID).OwnerUserID : 0;

					dest.PageCount = (int)Math.Ceiling((double)_presentationService.GetMessagesCount(src.ID) / dest.PageSize);
				});

			CreateMap<ChatRoomUser, ChatRoomUserViewModel>()
				.ForMember(dest => dest.UserName, opt => opt.MapFrom(src => _presentationService.GetUserFirstLastName(src.OwnerUserID)))
				.ForMember(dest => dest.PictureURL, opt => opt.MapFrom(src => _presentationService.GetUserPictureUrl(src.OwnerUserID)));

			CreateMap<Message, MessageViewModel>()
				.ForMember(dest => dest.SenderUserName, opt => opt.MapFrom(src => _presentationService.GetUserFirstLastName(src.OwnerUserID)))
				.ForMember(dest => dest.SenderPictureURL, opt => opt.MapFrom(src => _presentationService.GetUserPictureUrl(src.OwnerUserID)))
				.ForMember(dest => dest.Message, opt => opt.MapFrom(src => src.Content));
		}

		private void MapChatNotificationCriteria()
		{
			CreateMap<ChatNotification, ChatNotificationViewModel>()
				.ForMember(dest => dest.Read, opt => opt.MapFrom(src => src.ReadDate != null));
		}

		#endregion

	}
}
