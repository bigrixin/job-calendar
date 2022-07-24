using System.Collections.Generic;

namespace MyAbilityFirst.Infrastructure.Communication
{
	public interface IBroadcastService
	{
		void SendMessage(List<string> targetUserLogins, string content, int roomID);
		void SendStartTypingNotification(List<string> targetUserLogins, string content, int roomID);
		void SendStopTypingNotification(List<string> targetUserLogins, string content, int roomID);
		void SendAlert(List<string> targetUserLogins, string content, int notificationID);
	}
}
