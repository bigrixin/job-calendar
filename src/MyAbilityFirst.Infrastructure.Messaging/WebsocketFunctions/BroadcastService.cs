using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;
using System.Collections.Generic;

namespace MyAbilityFirst.Infrastructure.Communication
{
	public class BroadcastService : IBroadcastService
	{
		private readonly IHubContext _context;

		public BroadcastService(IConnectionManager manager)
		{
			this._context = manager.GetHubContext<SignalRHub>();
		}

		public void SendMessage(List<string> targetUserLogins, string content, int roomID)
		{
			var client = _context.Clients.Users(targetUserLogins);
			client.sendMessage(content, roomID);
		}

		public void SendStartTypingNotification(List<string> targetUserLogins, string content, int roomID)
		{
			var client = _context.Clients.Users(targetUserLogins);
			client.sendStartTypingNotification(content, roomID);
		}

		public void SendStopTypingNotification(List<string> targetUserLogins, string content, int roomID)
		{
			var client = _context.Clients.Users(targetUserLogins);
			client.sendStopTypingNotification(content, roomID);
		}

		public void SendAlert(List<string> targetUserLogins, string content, int notificationID)
		{
			var client = _context.Clients.Users(targetUserLogins);
			client.sendAlert(content, notificationID);
		}

	}
}