using Google.Apis.Calendar.v3.Data;
using MyAbilityFirst.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyAbilityFirst.Services.Common
{
	public interface ICalendarService
	{
		HttpResponseMessage CreateIcsFile();
		Event CreateGoogleCalendarEvents();
		string CreateGoogleCalendarById(Google.Apis.Calendar.v3.CalendarService service, string calendarId);
		Task<CalendarListEntry> CreateGoogleCalendarByName(Google.Apis.Calendar.v3.CalendarService service);
		CalendarListEntry ShowGoogleCalendarInList(Google.Apis.Calendar.v3.CalendarService service, string calendarId);
		CalendarListEntry GetGoogleCalendarById(Google.Apis.Calendar.v3.CalendarService service, string calendarId);
		Task<CalendarListEntry> GetGoogleCalendarByName(Google.Apis.Calendar.v3.CalendarService service);

		Google.Apis.Calendar.v3.Data.Event NewEvent();

		IList<CalendarEventsModel> GetCalendarEvents();
	}
}
