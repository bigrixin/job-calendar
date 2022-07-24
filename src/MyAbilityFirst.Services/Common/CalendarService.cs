using Google.Apis.Calendar.v3.Data;
using Ical.Net;
using Ical.Net.DataTypes;
using Ical.Net.Interfaces.DataTypes;
using Ical.Net.Serialization;
using Ical.Net.Serialization.iCalendar.Serializers;
using MyAbilityFirst.Domain;
using MyAbilityFirst.Infrastructure;
using MyAbilityFirst.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MyAbilityFirst.Services.Common
{
	public class CalendarService : ICalendarService
	{

		#region Fields

		private readonly IWriteEntities _entities;

		#endregion

		#region Ctor

		public CalendarService(IWriteEntities entities)
		{
			this._entities = entities;
		}

		#endregion

		#region Action

		public void GenerateIcsFile(Job job)
		{
			var now = DateTime.Now;
			var later = now.AddHours(1);

			//Repeat daily for 5 days
			var rrule = new RecurrencePattern(FrequencyType.Daily, 1) { Count = 5 };

			var e = new Ical.Net.Event
			{
				DtStart = new CalDateTime(now),
				DtEnd = new CalDateTime(later),

				RecurrenceRules = new List<IRecurrencePattern> { rrule },
			};

			var calendar = new Ical.Net.Calendar();
			calendar.Events.Add(e);

			var serializer = new CalendarSerializer(new SerializationContext());
			var serializedCalendar = serializer.SerializeToString(calendar);

		}

		public HttpResponseMessage CreateIcsFile()
		{
			string url = "http://abilityfirst.azurewebsites.net/";
			string summary = "Job from Ability First";   //

			// sets the calendar
			var calendar = new Ical.Net.Calendar();
			calendar.AddProperty("X-WR-CALNAME", summary);
			calendar.AddProperty("X-ORIGINAL-URL", url);
			calendar.AddProperty("METHOD", "PUBLISH");


			// set events
			var iCalEvent = new Ical.Net.Event()
			{
				DtStart = new CalDateTime(new DateTime(2017, 07, 17, 8, 0, 0, DateTimeKind.Local)),
				DtEnd = new CalDateTime(new DateTime(2017, 07, 18, 10, 0, 0, DateTimeKind.Local)),
				Created = new CalDateTime(DateTime.Now),
				Location = url,
				Summary = summary,
				Url = new Uri(url)
			};

			//set attendee
			var attendee = new Attendee
			{
				CommonName = "Rian Stockbower",   //add client or carer name
				Rsvp = true,
				Value = new Uri("mailto:myabilityfirst@gmail.com")      //user email client or carer
			};
			iCalEvent.Attendees = new List<IAttendee> { attendee };


			string description = "sample description";
			iCalEvent.AddProperty("X-ALT-DESC;FMTTYPE=text/html", description);
			calendar.Events.Add(iCalEvent);

			var serializer = new CalendarSerializer(new SerializationContext());
			var serializedCalendar = serializer.SerializeToString(calendar);
			var bytesCalendar = Encoding.UTF8.GetBytes(serializedCalendar);
			var result = new HttpResponseMessage(HttpStatusCode.OK)
			{
				Content = new ByteArrayContent(bytesCalendar)
			};

			result.Content.Headers.ContentDisposition =
					new ContentDispositionHeaderValue("attachment")
					{
						FileName = "abilityfistjobcalender.ics"
					};

			result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

			return result;
		}

		#endregion

		#region Google calendar

		public string CreateGoogleCalendarById(Google.Apis.Calendar.v3.CalendarService service, string calendarId)
		{
			var calendar = GetGoogleCalendarById(service, calendarId);
			if (calendar == null)
			{
				var newCalendar = new Google.Apis.Calendar.v3.Data.Calendar();
				newCalendar.Summary = ConfigurationManager.AppSettings.Get("CalendarSummary");
				newCalendar.TimeZone = ConfigurationManager.AppSettings.Get("CalendarTimeZone");
				var createdCalendar = service.Calendars.Insert(newCalendar).Execute();
				return createdCalendar.Id;
			}
			return calendar.Id;
		}

		public async Task<CalendarListEntry> CreateGoogleCalendarByName(Google.Apis.Calendar.v3.CalendarService service)
		{
			var calendar = await GetGoogleCalendarByName(service);
			if (calendar == null)
			{
				var newCalendar = new Google.Apis.Calendar.v3.Data.Calendar();
				newCalendar.Summary = ConfigurationManager.AppSettings.Get("CalendarSummary");
				newCalendar.TimeZone = ConfigurationManager.AppSettings.Get("CalendarTimeZone");
				var createdCalendar = service.Calendars.Insert(newCalendar).Execute();
				if (createdCalendar != null)
					return ShowGoogleCalendarInList(service, createdCalendar.Id);
			}
			return calendar;
		}

		public CalendarListEntry ShowGoogleCalendarInList(Google.Apis.Calendar.v3.CalendarService service, string calendarId)
		{
			CalendarListEntry calendarListEntry = new CalendarListEntry();
			calendarListEntry.Id = calendarId;
			var result = service.CalendarList.Insert(calendarListEntry).Execute();
			//var result = service.CalendarList.Update(calendarListEntry, calendarId).Execute();
			return result;
		}

		public CalendarListEntry GetGoogleCalendarById(Google.Apis.Calendar.v3.CalendarService service, string calendarId)
		{
			CalendarListEntry calendar = null;
			var calendars = service.CalendarList.List().Execute().Items;
			foreach (CalendarListEntry entry in calendars)
			{
				if (entry.Id == calendarId)
				{
					calendar = entry;
					break;
				}
			}
			return calendar;
		}

		public async Task<CalendarListEntry> GetGoogleCalendarByName(Google.Apis.Calendar.v3.CalendarService service)
		{

			var calendarSummary = ConfigurationManager.AppSettings.Get("CalendarSummary");
			var calendars = await service.CalendarList.List().ExecuteAsync();

			var calendar = calendars.Items.FirstOrDefault(c => c.Summary == calendarSummary);
			if (calendar != null)
				return calendar;
			return null;
		}

		//Generate google calendar event
		public Google.Apis.Calendar.v3.Data.Event CreateGoogleCalendarEvents()
		{
			Google.Apis.Calendar.v3.Data.Event calEvent = new Google.Apis.Calendar.v3.Data.Event
			{
				Summary = "The event is added by MyabilityFirst",
				Location = "Sydney",
				Start = new EventDateTime
				{
					DateTime = new DateTime(2017, 7, 25, 9, 00, 0)
				},
				End = new EventDateTime
				{
					DateTime = new DateTime(2017, 7, 25, 11, 59, 0)
				},
				Recurrence = new List<string>()
			};
			return calEvent;
		}

		public Google.Apis.Calendar.v3.Data.Event NewEvent()
		{
			Google.Apis.Calendar.v3.Data.Event newEvent = new Google.Apis.Calendar.v3.Data.Event()
			{
				Summary = "Cenozoic Ventures Event 2017",
				Location = "Cenozoic Ventures, 113 Reservoir St, Surry Hills New South Wales 2010",
				Description = "A chance to hear more about Cenozoic Ventures developer products.",
				Start = new EventDateTime()
				{
					DateTime = DateTime.Parse("2017-07-29T13:45:00+10:00"),
					TimeZone = ConfigurationManager.AppSettings.Get("CalendarTimeZone"),
				},
				End = new EventDateTime()
				{
					DateTime = DateTime.Parse("2017-07-29T15:00:00+10:00"),
					TimeZone = ConfigurationManager.AppSettings.Get("CalendarTimeZone"),
				},
				Recurrence = new String[] { "RRULE:FREQ=DAILY;COUNT=1" },
				Attendees = new EventAttendee[] {
				new EventAttendee() { Email = "myabilityfirst@gmail.com" },
				new EventAttendee() { Email = "bigrixin@hotmail.com" },

			//	new EventAttendee() { Email = "tlee7977@gmail.com" },
			//		new EventAttendee() { Email = "scott.cenozoic@gmail.com" },
	   	},
				Reminders = new Google.Apis.Calendar.v3.Data.Event.RemindersData()
				{
					UseDefault = false,
					Overrides = new EventReminder[] {
						new EventReminder() { Method = "email", Minutes = 24 * 60 },
						new EventReminder() { Method = "email", Minutes = 1 * 60 },
						new EventReminder() { Method = "sms", Minutes = 24 * 60 },
						new EventReminder() { Method = "sms", Minutes = 1 * 60 },
				}
				}
			};
			return newEvent;

		}

		#endregion

		#region FullCalender

		public IList<CalendarEventsModel> GetCalendarEvents()
		{
			var list = new List<CalendarEventsModel>();

			var random = new Random();
			for (int i = -5; i <= 5; i++)
			{
				var start = DateTime.Now.AddDays(i).AddHours(random.Next(-20, 20));
				list.Add
						(
								new CalendarEventsModel
								{
									ID = random.Next(1, 5000),
									Title = String.Format("Random Title {0}", random.Next(1, 500)),
									//Start = start,
									//End = start.AddHours(random.Next(1, 1)),
									Location = ""
								}
						);
			}
			return list;
		}

		#endregion
	}
}