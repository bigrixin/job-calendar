using AutoMapper;
using MyAbilityFirst.Domain;
using MyAbilityFirst.Infrastructure;
using MyAbilityFirst.Models;
using PagedList;
using System.Collections.Generic;
using System.Linq;

namespace MyAbilityFirst.Services.Common
{
	public class MessagesVMResolver : IValueResolver<ChatRoom, ChatRoomViewModel, List<MessageViewModel>>
	{

		private readonly IReadEntities _readEntities;
		public MessagesVMResolver(IReadEntities readEntities)
		{
			this._readEntities = readEntities;
		}

		public List<MessageViewModel> Resolve(ChatRoom src, ChatRoomViewModel dest, List<MessageViewModel> member, ResolutionContext context)
		{
			var pagedMessages = _readEntities.GetLazy<Message>(m => m.ChatRoomID == src.ID).OrderByDescending(m => m.ID).ToPagedList(dest.PageNumber, dest.PageSize).ToList();
			List<ChatRoomUserViewModel> userList = context.Mapper.Map<List<ChatRoomUserViewModel>>(src.ChatRoomUsers);
			List<MessageViewModel> res = context.Mapper.Map<List<MessageViewModel>>(pagedMessages);
			res = res.Select(m => {
				m.FormatAsSenderMessage = m.OwnerUserID == dest.RequestedUserID;
				m.SenderPictureURL = userList.Single(u => u.OwnerUserID == m.OwnerUserID).PictureURL;
				m.SenderUserName = userList.Single(u => u.OwnerUserID == m.OwnerUserID).UserName;
				return m;
			}).ToList();
			return res;
		}
	}
}
