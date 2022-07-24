using System;
using System.Collections.Generic;
using System.Linq;

namespace MyAbilityFirst.Domain
{
	public class Job
	{

		#region Properties

		public int ID { get; set; }
		public int ClientID { get; private set; }
		public string Title { get; set; }
		public string Description { get; set; }

		public Address Address { get; set; }
		public int PreferredGenderID { get; set; }

		public JobStatus JobStatus { get; set; }

		public virtual ICollection<Patient> Patients { get; set; }
		public virtual ICollection<Schedule> Schedules { get; set; }
		public virtual ICollection<CareWorker> CareWorkersViewed { get; set; }

		public DateTime? CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }

		#endregion

		#region Ctor

		protected Job()
		{
			// required by EF
			this.Address = new Address();
			this.Patients = new List<Patient>();
			this.Schedules = new List<Schedule>();
			this.CareWorkersViewed = new List<CareWorker>();
		}

		public Job(int clientID)
		{
			this.ClientID = clientID;
			this.Address = new Address();
			this.Patients = new List<Patient>();
			this.Schedules = new List<Schedule>();
			this.CreatedAt = DateTime.Now;
			this.UpdatedAt = DateTime.Now;
		}

		public void AddPatient(Patient patient)
		{
			this.Patients.Add(patient);
		}

		public Patient GetPatient(int patientID)
		{
			return this.Patients.Where(p => p.ID == patientID).SingleOrDefault();
		}

		public void UpdatePatient(Patient patient)
		{
			var item = this.GetPatient(patient.ID);
			if (item != null)
			{
				this.Patients.Remove(item);
				this.Patients.Add(patient);
			}
		}

		public Patient DeletePatient(int patientID)
		{
			var item = this.GetPatient(patientID);
			if (item != null)
			{
				this.Patients.Remove(item);
				return item;
			}
			return null;
		}

		public void AddSchedule(Schedule schedule)
		{
			this.Schedules.Add(schedule);
		}

		public Schedule GetSchedule(int scheduleID)
		{
			return this.Schedules.Where(s => s.ID == scheduleID).SingleOrDefault();
		}

		public void UpdateSchedule(Schedule schedule)
		{
			var item = GetSchedule(schedule.ID);
			if (item != null)
			{
				this.Schedules.Remove(item);
				this.Schedules.Add(schedule);
			}
		}

		public Schedule DeleteSchedule(int scheduleID)
		{
			var item = GetSchedule(scheduleID);
			if (item != null)
			{
				this.Schedules.Remove(item);
				return item;
			}
			return null;
		}

		public void ClearPatients()
		{
			this.Patients.Clear();
		}

		public void ClearSchedules()
		{
			this.Schedules.Clear();
		}

		#endregion

		#region Careworker

		public void AddCareWorkerViewed(CareWorker carer)
		{
			if (GetViewedCareWorker(carer.ID) == null)
				this.CareWorkersViewed.Add(carer);
		}

		public CareWorker GetViewedCareWorker(int carerID)
		{
			return this.CareWorkersViewed.Where(s => s.ID == carerID).SingleOrDefault();
		}

		public int GetCareWorkerViewedCounter()
		{
			return this.CareWorkersViewed.Count();
		}

		#endregion

	}
}