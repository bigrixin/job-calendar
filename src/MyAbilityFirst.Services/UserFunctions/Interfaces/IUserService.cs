using MyAbilityFirst.Domain;
using System.Collections.Generic;


namespace MyAbilityFirst.Services.Common
{
	public interface IUserService
	{
		// User Services
		void CreateClient(string loginIdentityID, string email);
		void CreateCareWorker(string loginIdentityID, string email);
		void CreateCoordinator(string loginIdentityID, string email);
		User GetUser(int id);
		User GetLoggedInUser();
		string GetUserName(int id);
		string GetUserFirstLastName(int id);
		string GetLoggedInUserType();
		bool EmailVerified(string loginIdentityID);

		// Shortlist Services
		Shortlist CreateShortlist(int ownerUserID, Shortlist shortlistData);
		Shortlist RetrieveShortlistBySelectedUserID(int ownerUserID, int selectedUserID);
		List<Shortlist> RetrieveAllShortlists(int ownerUserID);
		Shortlist UpdateShortlist(int ownerUserID, Shortlist shortlistData);

		// Category Services
		UserSubcategory CreateUserSubcategory(int ownerUserID, UserSubcategory userSubcategoryData);
		UserSubcategory RetrieveUserSubcategoryBySubcategoryID(int ownerUserID, int subcategoryID);
		List<UserSubcategory> RetrieveAllUserSubcategories(int ownerUserID);
		UserSubcategory UpdateUserSubcategory(int ownerUserID, UserSubcategory userSubcategoryData);
		List<UserSubcategory> ReplaceAllUserSubCategories(int ownerUserID, int[] postedSubCategoryIDs, List<UserSubcategory> customValueList);

		// Notification Services
		NotificationType CreateNotification<NotificationType>(NotificationType notificationData) where NotificationType : Notification;
		ChatNotification RetrieveChatNotification(int ownerUserID, int roomID);
		NotificationType RetrieveNotification<NotificationType>(int notificationID) where NotificationType : Notification;
		NotificationType UpdateNotification<NotificationType>(NotificationType notificationData) where NotificationType : Notification;
		NotificationType SetNotificationAsRead<NotificationType>(NotificationType notificationData) where NotificationType : Notification;
		NotificationType SetNotificationAsClosed<NotificationType>(NotificationType notificationData) where NotificationType : Notification;
		List<NotificationType> RetrieveOpenNotifications<NotificationType>(int ownerUserID) where NotificationType : Notification;
		void SendNotification(int ownerUserID, string notificationHtmlContent, int notificationID);
	}
}