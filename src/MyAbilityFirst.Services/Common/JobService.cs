using AutoMapper;
using Google.Apis.Calendar.v3.Data;
using Ical.Net;
using Ical.Net.DataTypes;
using Ical.Net.Interfaces.DataTypes;
using Ical.Net.Serialization;
using Ical.Net.Serialization.iCalendar.Serializers;
using MyAbilityFirst.Domain;
using MyAbilityFirst.Infrastructure;
using MyAbilityFirst.Infrastructure.Auth;
using MyAbilityFirst.Models;
using MyAbilityFirst.Services.CareWorkerFunctions;
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
	public class JobService : UserService, IJobService
	{

		#region Fields

		private readonly IWriteEntities _entities;
		private readonly IMapper _mapper;
		private readonly ICareWorkerService _careWorkerServices;
		private readonly IUserService _userServices;
		#endregion

		#region Ctor

		public JobService(IWriteEntities entities, IMapper mapper, ICareWorkerService careWorkerService, IUserService userServices) : base(entities, mapper)
		{
			this._entities = entities;
			this._mapper = mapper;
			this._careWorkerServices = careWorkerService;
			this._userServices = userServices;
		}

		#endregion

		#region Carer

		public List<JobsViewModel> GetPublicJobsViewModelList()
		{
			var jobs = this._entities.Get<Job>().OrderByDescending(a => a.CreatedAt);
			var jobsVM = new List<JobsViewModel>();
			jobs.ToList().ForEach(j =>
			{
				jobsVM.Add(mapJobToViewModel(j));
			});
			return jobsVM;
		}

		public JobsViewModel GetJobViewModel(int id)
		{
			var job = this._entities.Get<Job>(a => a.ID == id).SingleOrDefault();
			if (job == null)
				return null;
			AddCareView(job);
			return mapJobToViewModel(job);
		}

		#endregion

		#region Helper

		private void AddCareView(Job job)
		{
			var carer = this._careWorkerServices.RetrieveCareWorker(this._userServices.GetLoggedInUser().ID);
			if (carer!=null)
			{
			  
				job.AddCareWorkerViewed(carer);
				this._entities.Update(job);
				this._entities.Save();
			}
		}

		private JobsViewModel mapJobToViewModel(Job job)
		{
			JobsViewModel model = new JobsViewModel();

			model = _mapper.Map<Job, JobsViewModel>(job);
			model.Views = job.GetCareWorkerViewedCounter();
			if (job.Schedules.Count() != 0)
			{
				var every = "";
				var dayOfWeek = "";
				var endDate = "";
				var byDay = "";
				var schedule = job.Schedules.FirstOrDefault();
				var startTime = schedule.Duration.Start.ToString("HH:mm tt");
				var endTime = schedule.Duration.End.ToString("HH:mm tt");
				if (schedule.EffectiveEndDate != null)
					endDate = String.Format("{0:dddd, MMMM d, yyyy}", schedule.EffectiveEndDate);
				if (schedule.Interval != 1)
				{
					var scheduleType = schedule.ScheduleType.ToString();
					every = "Every " + schedule.Interval + " " + scheduleType.Remove(scheduleType.Length - 2) + "s, on ";
				}

				switch (schedule.ScheduleType)
				{
					case ScheduleType.OneOff:
						byDay = endDate + ", one-off";
						break;
					case ScheduleType.Weekly:
						foreach (var element in job.Schedules)
							dayOfWeek = dayOfWeek + ", " + Enum.GetName(typeof(DayOfWeek), element.GetDayOfWeek());
						if (dayOfWeek != "")
						{
							dayOfWeek = dayOfWeek.TrimStart(',');
							var pos = dayOfWeek.LastIndexOf(',');
							if (pos > 0)
								dayOfWeek = dayOfWeek.Substring(0, pos) + " &" + dayOfWeek.Substring(pos + 1);
						}
						byDay = (endDate != "") ? every + " " + dayOfWeek + ", end on " + endDate : every + " " + dayOfWeek;
						break;
					case ScheduleType.Monthly:
						var day = "on " + Enum.GetName(typeof(FrequencyMonthlyType), schedule.Position);
						if (schedule.Position == 0)
							day = day + " " + schedule.Duration.Start.Day.ToString();
						else
						{
							switch (schedule.ByDay)
							{
								case -1:
									break;
								case 8:
									day = day + " Weekday";
									break;
								case 9:
									day = day + " Weekend day";
									break;
								default:
									day = day + " " + Enum.GetName(typeof(DayOfWeek), schedule.ByDay);
									break;
							}
						}
						byDay = (endDate != "") ? every + " " + day + ", " + schedule.ScheduleType + ", end on " + endDate : every + " " + day + ", " + schedule.ScheduleType;
						break;
				}

				if (schedule.EffectiveEndDate == null)
					byDay = byDay + ", days without end.";

				model.Schedule = startTime + " - " + endTime + ", " + byDay;
				if (schedule.EffectiveEndDate != null)
					model.EndDate = (DateTime)schedule.EffectiveEndDate;
				else
					model.EndDate = DateTime.Now.AddYears(3);
			}

			var patients = "";
			foreach (var element in job.Patients)
				patients = patients + ", " + element.FirstName;
			if (patients != "")
				patients = patients.TrimStart(',');
			model.Patients = patients;
			return model;
		}

		#endregion
	}
}