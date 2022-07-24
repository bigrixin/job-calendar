using System;
using System.Collections.Generic;
using Ical.Net;
using Ical.Net.DataTypes;
using Ical.Net.Interfaces.DataTypes;
using System.Linq;
using MyAbilityFirst.Domain.Models.ValueObject;


namespace MyAbilityFirst.Domain
{
	public class Schedule
	{

		#region Properties

		public int ID { get; set; }
		public int JobID { get; set; }
		public Duration Duration { get; private set; }
		public virtual ICollection<Booking> Bookings { get; set; }
		public ScheduleType ScheduleType { get; private set; }
		public DateTime? EffectiveEndDate { get; private set; }
		public int Interval { get; set; }
		public int Position { get; set; }
		public int ByDay { get; set; }

		#endregion

		#region Ctor

		protected Schedule()
		{
			// required by EF
			this.Bookings = new List<Booking>();
		}

		public Schedule(DateTime start, DateTime end, ScheduleType scheduleType, int interval, int position, int byDay, DateTime? effectiveEndDate = null)
		{
			if (start >= end)
				throw new ArgumentOutOfRangeException("start", "The DateTime value for 'start' must be less than 'end'.");

			this.Interval = interval;
			this.Position = position;
			this.ByDay = byDay;
			this.Duration = new Duration(start, end);
			this.ScheduleType = scheduleType;
			this.EffectiveEndDate = (scheduleType == ScheduleType.OneOff || scheduleType == ScheduleType.OneOffReplacement) ? end : effectiveEndDate;
			this.Bookings = new List<Booking>();
		}

		#endregion

		public bool Overlaps(Schedule otherSchedule, DateTime searchStart, DateTime searchEnd)
		{
			return ICalOverlaps(ConvertToICal(this), ConvertToICal(otherSchedule), searchStart, searchEnd);
		}

		public List<Duration> GetScheduledDurations(DateTime searchStart, DateTime searchEnd)
		{
			return GetICalScheduledOccurences(searchStart, searchEnd);
		}

		public DateTime GetStartDate()
		{
			return Duration.Start;
		}

		public DateTime? GetEndDate()
		{
			return EffectiveEndDate;
		}

		public TimeSpan GetStartTime()
		{
			return Duration.Start.TimeOfDay;
		}

		public TimeSpan GetEndTime()
		{
			return Duration.End.TimeOfDay;
		}

		public int GetDayOfWeek()
		{
			return (int)Duration.Start.DayOfWeek;
		}


		#region Helpers

		private List<Duration> GetICalScheduledOccurences(DateTime searchStart, DateTime searchEnd)
		{
			var c = ConvertToICal(this);
			HashSet<Occurrence> x = c.GetOccurrences(searchStart, searchEnd);
			return x.Select(o => { Duration p = new Duration(o.Period.StartTime.AsSystemLocal, o.Period.EndTime.AsSystemLocal); return p; }).ToList();
		}

		private bool ICalOverlaps(Calendar c1, Calendar c2, DateTime searchStart, DateTime searchEnd)
		{
			c1.GetOccurrences(searchStart, searchEnd).IntersectWith(c2.GetOccurrences(searchStart, searchEnd));
			return c1.GetOccurrences(searchStart, searchEnd).Count > 0;
		}

		private Calendar ConvertToICal(Schedule schedule)
		{
			Calendar c = new Calendar();
			IWeekDay w = new WeekDay();
			bool hasDayOfWeek = false;
			int dayOfMonth = 0;
			FrequencyType ft = FrequencyType.None;
			switch (schedule.ScheduleType)
			{
				case ScheduleType.OneOff:

				case ScheduleType.OneOffReplacement:
					c.Events.Add(new Event
					{
						DtStart = new CalDateTime(schedule.Duration.Start),
						DtEnd = new CalDateTime(schedule.Duration.End),
					});
					return c;
				case ScheduleType.Weekly:
					ft = FrequencyType.Weekly;
					hasDayOfWeek = true;
					w.DayOfWeek = this.Duration.Start.DayOfWeek;
					break;
				case ScheduleType.Monthly:
					ft = FrequencyType.Monthly;
					hasDayOfWeek = true;
					w.DayOfWeek = this.Duration.Start.DayOfWeek;
					if (this.Position == 0)
						dayOfMonth = this.Duration.Start.Day;
					break;
				default:
					break;
			}

			Event e = new Event
			{
				DtStart = new CalDateTime(schedule.Duration.Start),
				DtEnd = new CalDateTime(schedule.Duration.End),
				RecurrenceRules = new List<IRecurrencePattern>(),
			};

			RecurrencePattern rp = new RecurrencePattern(ft, this.Interval);
			if (this.EffectiveEndDate != null)
				rp.Until = (DateTime)this.EffectiveEndDate;
			if (this.Position != 0)
				rp.BySetPosition = new List<int>() { this.Position };
			if (Position == 0 && schedule.ScheduleType == ScheduleType.Monthly)
				rp.ByMonthDay = new List<int> { dayOfMonth };
			else if (hasDayOfWeek)
				rp.ByDay = new List<IWeekDay>() { w };

			if (this.ByDay == 9)
				rp.ByDay = new List<IWeekDay>()
				{
					new WeekDay(DayOfWeek.Saturday),
					new WeekDay(DayOfWeek.Sunday)
				 };
			else if (this.ByDay == 8)
				rp.ByDay = new List<IWeekDay>()
				 {
					 new WeekDay(DayOfWeek.Monday),
					 new WeekDay(DayOfWeek.Tuesday),
					 new WeekDay(DayOfWeek.Wednesday),
					 new WeekDay(DayOfWeek.Thursday),
					 new WeekDay(DayOfWeek.Friday),
				 };

			e.RecurrenceRules.Add(rp);
			c.Events.Add(e);
			return c;
		}

		#endregion

	}
}