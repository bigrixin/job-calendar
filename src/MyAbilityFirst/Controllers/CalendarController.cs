using Google.Apis.Auth.OAuth2.Mvc;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using MyAbilityFirst.Services.Common;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using static Google.Apis.Auth.OAuth2.Web.AuthorizationCodeWebApp;

namespace MyAbilityFirst.Controllers
{
	//[AllowAnonymous]
	[Authorize]

	public class CalendarController : Controller
	{

		#region Fields

		private readonly ICalendarService _calendarServices;

		#endregion

		#region Ctor

		public CalendarController(ICalendarService calendarServices)
		{
			this._calendarServices = calendarServices;
		}

		#endregion
		// GET: Calendar
		public ActionResult Index()
		{
			return View();
		}


		//public string[] Scopes
		//{
		//	get
		//	{
		//		return new[] {
		//		"https://www.googleapis.com/auth/calendar",
		//								//"openid",
		//								//"email",
		//								CalendarService.Scope.CalendarReadonly,  //
		//						};
		//	}
		//}

		//	private readonly IDataStore dataStore = new FileDataStore(GoogleWebAuthorizationBroker.Folder);

		//private async Task<UserCredential> GetCredentialForApiAsync()
		//{
		//	var initializer = new GoogleAuthorizationCodeFlow.Initializer
		//	{
		//		ClientSecrets = new ClientSecrets
		//		{
		//			ClientId = ConfigurationManager.AppSettings.Get("GoogleClientId"),
		//			ClientSecret = ConfigurationManager.AppSettings.Get("GoogleClientSecret"),
		//		},
		//		Scopes = Scopes,
		//	};
		//	var flow = new GoogleAuthorizationCodeFlow(initializer);

		//	var claims = ClaimsPrincipal.Current.Claims;
		//	var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);

		//	//var identity = await HttpContext.GetOwinContext().Authentication.GetExternalIdentityAsync(
		//	//	DefaultAuthenticationTypes.ApplicationCookie);
		//	var userId = identity.FindFirstValue(RequestedScopes.GoogleUserId);

		//	var token = await dataStore.GetAsync<TokenResponse>(userId);
		//	return new UserCredential(flow, userId, token);
		//}

		//// GET: /Calendar/UpcomingEvents
		//public async Task<ActionResult> UpcomingEvents()
		//{
		//	var claims = ClaimsPrincipal.Current.Claims;
		//	var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
		//	var userId = identity.FindFirstValue(RequestedScopes.GoogleUserId);
		//	if (userId == null)
		//		return RedirectToAction("GoogleLogin", "Account");

		//	if (!Request.IsAuthenticated)
		//	{
		//		return RedirectToAction("GoogleLogin", "Account");
		//	}
		//	const int MaxEventsPerCalendar = 20;
		//	const int MaxEventsOverall = 50;

		//	var model = new CalendarViewModel();

		//	var credential = await GetCredentialForApiAsync();

		//	var initializer = new BaseClientService.Initializer()
		//	{
		//		HttpClientInitializer = credential,
		//		ApplicationName = "AbilityFirstCalendar",
		//	};
		//	var service = new Google.Apis.Calendar.v3.CalendarService(initializer);

		//	// Fetch the list of calendars.
		//	var calendars = await service.CalendarList.List().ExecuteAsync();

		//	// Fetch some events from each calendar.
		//	var fetchTasks = new List<Task<Google.Apis.Calendar.v3.Data.Events>>(calendars.Items.Count);
		//	foreach (var calendar in calendars.Items)
		//	{
		//		var request = service.Events.List(calendar.Id);
		//		request.MaxResults = MaxEventsPerCalendar;
		//		request.SingleEvents = true;
		//		request.TimeMin = DateTime.Now;
		//		fetchTasks.Add(request.ExecuteAsync());
		//	}
		//	var fetchResults = await Task.WhenAll(fetchTasks);

		//	// Sort the events and put them in the model.
		//	var upcomingEvents = from result in fetchResults
		//											 from evt in result.Items
		//											 where evt.Start != null
		//											 let date = evt.Start.DateTime.HasValue ?
		//													 evt.Start.DateTime.Value.Date :
		//													 DateTime.ParseExact(evt.Start.Date, "yyyy-MM-dd", null)
		//											 let sortKey = evt.Start.DateTimeRaw ?? evt.Start.Date
		//											 orderby sortKey
		//											 select new { evt, date };
		//	var eventsByDate = from result in upcomingEvents.Take(MaxEventsOverall)
		//										 group result.evt by result.date into g
		//										 orderby g.Key
		//										 select g;

		//	var eventGroups = new List<CalendarEventModel>();
		//	foreach (var grouping in eventsByDate)
		//	{
		//		eventGroups.Add(new CalendarEventModel
		//		{
		//			GroupTitle = grouping.Key.ToLongDateString(),
		//			Events = grouping,
		//		});
		//	}

		//	model.EventGroups = eventGroups;
		//	return View(model);
		//}

		//// GET: /Calendar/AddEvents
		//[AllowAnonymous]
		//public async Task<ActionResult> AddEvent1()
		//{
		//	//var claims = ClaimsPrincipal.Current.Claims;
		//	//var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
		//	//var userId = identity.FindFirstValue(RequestedScopes.GoogleUserId);
		//	//if (userId == null)
		//	//	return RedirectToAction("GoogleLogin", "Account");


		//	//var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
		//	//													 new ClientSecrets
		//	//													 {
		//	//														 ClientId = ConfigurationManager.AppSettings.Get("GoogleClientId"),
		//	//														 ClientSecret = ConfigurationManager.AppSettings.Get("GoogleClientSecret"),
		//	//													 },
		//	//													 new[] { CalendarService.Scope.Calendar },
		//	//													 "user",
		//	//													 CancellationToken.None).Result;

		//	UserCredential credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
		//						new ClientSecrets
		//						{
		//							ClientId = ConfigurationManager.AppSettings.Get("GoogleClientId"),
		//							ClientSecret = ConfigurationManager.AppSettings.Get("GoogleClientSecret"),
		//						},
		//						new[] { CalendarService.Scope.Calendar },
		//						"user",
		//						CancellationToken.None).Result;

		//	var service = new CalendarService(new BaseClientService.Initializer()
		//	{
		//		HttpClientInitializer = credential,
		//		ApplicationName = "Calendar API Sample",
		//	});
		//	var myEvent = new Event
		//	{
		//		Summary = "Google Calendar Api Sample Code by Mukesh Salaria",
		//		Location = "Gurdaspur, Punjab, India",
		//		Start = new EventDateTime
		//		{
		//			DateTime = new DateTime(2017, 7, 20, 6, 0, 0),
		//		},
		//		End = new EventDateTime
		//		{
		//			DateTime = new DateTime(2017, 7, 21, 7, 30, 0),
		//		},
		//		Recurrence = new String[] { "RRULE:FREQ=WEEKLY;BYDAY=MO" },
		//		Attendees = new List<EventAttendee>()
		//						{
		//							 new EventAttendee (){ Email = "programmer.mukesh01@gmail.com"}
		//						},
		//	};

		//	var eventResult = await service.Events.Insert(myEvent, "primary").ExecuteAsync();
		//	//var recurringEvent = service.Events.Insert(myEvent, "primary");
		//	//recurringEvent.SendNotifications = true;
		//	//recurringEvent.ExecuteAsync();
		//	return View();
		//}


		//[HttpGet, Route("calendar/addevent2")]
		//public async Task<ActionResult> AddEvent2()
		//{
		//	var claims = ClaimsPrincipal.Current.Claims;
		//	var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
		//	var userId = identity.FindFirstValue(RequestedScopes.GoogleUserId);
		//	if (userId == null)
		//		return RedirectToAction("GoogleLogin", "Account");

		//	var credential = await GetCredentialForApiAsync();



		//	var initializer = new BaseClientService.Initializer()
		//	{
		//		HttpClientInitializer = credential,
		//		ApplicationName = "AbilityFirstCalendar",
		//	};
		//	var service = new CalendarService(initializer);

		//	// Fetch the list of calendars.
		//	var calendars = await service.CalendarList.List().ExecuteAsync();

		//	var calendar = calendars.Items.SingleOrDefault(c => c.Summary == "AbilityFirst");   // AbilityFirst or  primary
		//	var events = service.Events.List("Calendar Id");

		//	if (calendar != null)
		//	{
		//		Event calEvent = new Event
		//		{
		//			Summary = "Awesome Party",
		//			Location = "My House",
		//			Start = new EventDateTime
		//			{
		//				DateTime = new DateTime(2017, 7, 20, 19, 00, 0)
		//			},
		//			End = new EventDateTime
		//			{
		//				DateTime = new DateTime(2017, 7, 21, 23, 59, 0)
		//			},
		//			Recurrence = new List<string>()
		//		};

		//		var eventResult = await service.Events.Insert(calEvent, calendar.Id).ExecuteAsync();
		//		//	Event recurringEvent = service.Events.Insert(calEvent, "primary").Execute();
		//	}
		//	return View();
		//}

		[AllowAnonymous]
		public ActionResult AddEvent3()
		{
			return View();
		}

		[AllowAnonymous]
		public async Task<ActionResult> AddEventToGoogleCalendar(CancellationToken cancellationToken)
		{
			var result = await new AuthorizationCodeMvcApp(this, new AppFlowMetadata()).AuthorizeAsync(cancellationToken);

			if (result.Credential == null)
				return new RedirectResult(result.RedirectUri);
			var service = getGoogleCalendarService(result);

		 	var calendar =await this._calendarServices.CreateGoogleCalendarByName(service);

			if (calendar != null)
			{
				//var calEvent = this._calendarServices.CreateGoogleCalendarEvents();
				var calEvent = this._calendarServices.NewEvent();
				var eventResult = await service.Events.Insert(calEvent, calendar.Id.ToString()).ExecuteAsync();
			}

		//	await addEventToGoogleCalendar(service);
			//if (result.Credential.Token.IsExpired(SystemClock.Default))
			//{
			//	TokenResponse token = new TokenResponse();
			//	//If the token is expired recreate the token
			//	token = await result.Credential.Flow.RefreshTokenAsync(ConfigHelper.GetConfig().ToString(), result.Credential.Token.RefreshToken, CancellationToken.None);

			//	//Get the authorization details back
			//	result = await new AuthorizationCodeMvcApp(this, new AppFlowMetadata()).AuthorizeAsync(cancellationToken);
			//}

			return View();
		}


		[AllowAnonymous]
		public ActionResult fullcalendar()
		{
			return View();
		}


		[AllowAnonymous]
		public async Task<ActionResult> ListEvents(CancellationToken cancellationToken)
		{
			var result = await new AuthorizationCodeMvcApp(this, new AppFlowMetadata()).AuthorizeAsync(cancellationToken);

			if (result.Credential == null)
				return new RedirectResult(result.RedirectUri);
			var service = getGoogleCalendarService(result);

			// Define parameters of request.
			EventsResource.ListRequest request = service.Events.List("primary"); // calendar ID or  primary		
			request.TimeMin = DateTime.Now;
			request.ShowDeleted = false;
			request.SingleEvents = true;
			request.MaxResults = 10;
			request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;
			// List events.
			Events events = await request.ExecuteAsync();
			System.Diagnostics.Debug.WriteLine("Upcoming events:");
			if (events.Items != null && events.Items.Count > 0)
			{
				foreach (var eventItem in events.Items)
				{
					string when = eventItem.Start.DateTime.ToString();
					if (String.IsNullOrEmpty(when))
					{
						when = eventItem.Start.Date;
					}
					Debug.WriteLine("{0} ({1})", eventItem.Summary, when);
				}
			}
			else
			{
				Debug.WriteLine("No upcoming events found.");
			}
			//	Console.Read();
			return View();
		}

		private async Task addEventToGoogleCalendar(Google.Apis.Calendar.v3.CalendarService service)
		{
			var calendar = await this._calendarServices.GetGoogleCalendarByName(service);
			if (calendar != null)
			{
				var calEvent = this._calendarServices.CreateGoogleCalendarEvents();
				var eventResult = await service.Events.Insert(calEvent, calendar.Id).ExecuteAsync();
			}
		}

		private Google.Apis.Calendar.v3.CalendarService getGoogleCalendarService(AuthResult result)
		{
			var initializer = new BaseClientService.Initializer()
			{
				HttpClientInitializer = result.Credential,
				ApplicationName = ConfigurationManager.AppSettings.Get("CalendarApplicationName")
			};
			return new Google.Apis.Calendar.v3.CalendarService(initializer);
		}
	}
}