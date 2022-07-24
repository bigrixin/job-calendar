using MyAbilityFirst.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace MyAbilityFirst.Models
{
	public class NewJobScheduleViewModel
	{

		[Display(Name = "Start Date *")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
		public DateTime? StartDate { get; set; }

		[Display(Name = "Start Time *")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm A}")]
		public string StartTime { get; set; }

		[Display(Name = "End Time *")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:hh:mm A}")]
		public string EndTime { get; set; }

		[Display(Name = "Start Date *")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
		public DateTime? RecurringStartDate { get; set; }

		[Display(Name = "End Date")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
		public DateTime? RecurringEndDate { get; set; }

		[Display(Name = "Start Time *")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm A}")]
		public string RecurringStartTime { get; set; }

		[Display(Name = "End Time *")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:hh:mm A}")]
		public string RecurringEndTime { get; set; }


		[Display(Name = "Is This Job Reoccuring? ")]
		public bool Recurring { get; set; }

		[Display(Name = "Frequency *")]
		public ScheduleType Frequency { get; set; }

		[Display(Name = "Every")]
		public int SequenceMonthly { get; set; }
		[Display(Name = "On")]
		public int FrequencyMonthly { get; set; }

		[Display(Name = "Day")]
		[Range(0, 31, ErrorMessage = "Day has to be between 1-31")]
		public int Day { get; set; }

		[Display(Name = "Every")]
		public int SequenceWeekly { get; set; }

		public int DayofWeeks { get; set; }

		[Required(ErrorMessage = "Please select at least one option")]
		public List<bool> SelectedDayofWeek { get; set; }

		public string EndDateOption { get; set; }

		[Display(Name = "Occurrence/s")]
		public int Occurrences { get; set; }
		public string EventsDate { get; set; }
		public IEnumerable<SelectListItem> DayofWeekList
		{
			get
			{
				return Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>()
						.Select(a => new SelectListItem { Value = ((int)a).ToString(), Text = a.ToString() });
			}
		}

		public SelectList DayofWeekDropdownList
		{
			get
			{
				var dayOfWeekList = Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>()
						.Select(a => new SelectListItem { Value = ((int)a).ToString(), Text = a.ToString() });
				return new SelectList((dayOfWeekList).ToList(), "Value", "Text");
			}
		}

		public IEnumerable<SelectListItem> DayofWeekActionList
		{
			get
			{
				var result = Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>()
						.Select(a => new SelectListItem { Value = ((int)a).ToString(), Text = a.ToString() }).ToList();
				result.Insert(0, new SelectListItem { Value = "7", Text = "Day" });
				result.Insert(1, new SelectListItem { Value = "8", Text = "Weekday" });
				result.Insert(2, new SelectListItem { Value = "9", Text = "Weekend Day" });
				return result;
			}
		}

		public IEnumerable<SelectListItem> FrequencyMonthlyList
		{
			get
			{
				return Enum.GetValues(typeof(FrequencyMonthlyType)).Cast<FrequencyMonthlyType>()
						.Select(a => new SelectListItem { Value = ((int)a).ToString(), Text = a.ToString() });

			}
		}


	}
}