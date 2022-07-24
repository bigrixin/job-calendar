using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Mvc;
using Google.Apis.Calendar.v3;
using Google.Apis.Util.Store;
using System;
using System.Configuration;
using System.Web.Mvc;

namespace MyAbilityFirst
{
	public class AppFlowMetadata : FlowMetadata
	{
		private IAuthorizationCodeFlow flow { get; set; }

		public AppFlowMetadata() : base()
		{
			flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
			{
				ClientSecrets = new ClientSecrets
				{
					ClientId = ConfigurationManager.AppSettings.Get("GoogleClientId"),
					ClientSecret = ConfigurationManager.AppSettings.Get("GoogleClientSecret")
				},
				Scopes = new[] { CalendarService.Scope.Calendar },
				DataStore = new FileDataStore("Calendar.Api.Auth.Store")
			});
		}

		public override string GetUserId(Controller controller)
		{
			var user = controller.Session["user"];
			if (user == null)
			{
				user = Guid.NewGuid();
				controller.Session["user"] = user;
			}
			return user.ToString();
		}

		public override IAuthorizationCodeFlow Flow
		{
			get { return flow; }
		}

		public override string AuthCallback
		{
			get
			{
				//default: AuthCallback/IndexAsync
				return @"/GoogleAuthCallback/IndexAsync";
			}
		}
	}

}