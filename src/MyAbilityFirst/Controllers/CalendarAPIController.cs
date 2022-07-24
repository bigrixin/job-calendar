using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using MyAbilityFirst.Services.Common;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using Google.Apis.Util.Store;
using System.IO;
using System;
using System.Web;


namespace MyAbilityFirst.Controllers
{

	public class CalendarAPIController : ApiController
	{

		#region Fields

		private readonly ICalendarService _calendarServices;


		#endregion

		#region Ctor

		public CalendarAPIController(ICalendarService calendarServices)
		{
			this._calendarServices = calendarServices;
		}

		#endregion

		[System.Web.Http.Route("api/getcalendar")]
		[System.Web.Http.HttpGet]
		public HttpResponseMessage GetFile()
		{
			var result = this._calendarServices.CreateIcsFile();
			return result;
		}

		[System.Web.Http.Route("api/gecalendar")]
		public IHttpActionResult calendar()
		{
			// If modifying these scopes, delete your previously saved credentials
			// at ~/.credentials/calendar-dotnet-quickstart.json
			string[] Scopes = { Google.Apis.Calendar.v3.CalendarService.Scope.CalendarReadonly };
			string ApplicationName = "Google Calendar API .NET Quickstart";

			UserCredential credential;
			String strPathAndQuery = HttpContext.Current.Request.Url.PathAndQuery;
			String strUrl = HttpContext.Current.Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
			using (var stream =
					new FileStream("google_client_secret.json", FileMode.Open, FileAccess.Read))
			{
				string credPath = System.Environment.GetFolderPath(
						System.Environment.SpecialFolder.Personal);
				credPath = Path.Combine(credPath, ".credentials/calendar-dotnet-quickstart.json");

				credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
						GoogleClientSecrets.Load(stream).Secrets,
						Scopes,
						"user",
						CancellationToken.None,
						new FileDataStore(credPath, true)).Result;
				Console.WriteLine("Credential file saved to: " + credPath);
			}

			// Create Google Calendar API service.
			var service = new Google.Apis.Calendar.v3.CalendarService(new BaseClientService.Initializer()
			{
				HttpClientInitializer = credential,
				ApplicationName = ApplicationName,
			});

			// Define parameters of request.
			EventsResource.ListRequest request = service.Events.List("primary");
			request.TimeMin = DateTime.Now;
			request.ShowDeleted = false;
			request.SingleEvents = true;
			request.MaxResults = 10;
			request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

			// List events.
			Events events = request.Execute();
			Console.WriteLine("Upcoming events:");
			if (events.Items != null && events.Items.Count > 0)
			{
				foreach (var eventItem in events.Items)
				{
					string when = eventItem.Start.DateTime.ToString();
					if (String.IsNullOrEmpty(when))
					{
						when = eventItem.Start.Date;
					}
					Console.WriteLine("{0} ({1})", eventItem.Summary, when);
				}
			}
			else
			{
				Console.WriteLine("No upcoming events found.");
			}
			Console.Read();



			return null;
		}


	}
}
