using System;
using System.Collections.Generic;
using System.Linq;

namespace MyAbilityFirst.Domain
{
	public class Client : User
	{

		#region Properties

		public Disclaimers Disclaimers { get; set; }
		public NotificationSettings NotificationSettings { get; set; }

		public virtual ICollection<Patient> Patients { get; set; }
		public virtual ICollection<Job> Jobs { get; set; }
		public virtual ICollection<Booking> Bookings { get; set; }

		#endregion

		#region Ctor

		protected Client()
		{
			// required by EF
			this.Disclaimers = new Disclaimers();
			this.NotificationSettings = new NotificationSettings();
			this.Patients = new List<Patient>();
			this.Jobs = new List<Job>();
			this.Shortlists = new List<Shortlist>();
			this.Bookings = new List<Booking>();
		}

		public Client(string loginIdentityId) : base(loginIdentityId)
		{
			this.Disclaimers = new Disclaimers();
			this.NotificationSettings = new NotificationSettings();
			this.Patients = new List<Patient>();
			this.Jobs = new List<Job>();
			this.Shortlists = new List<Shortlist>();
			this.Bookings = new List<Booking>();
		}

		#endregion

		#region patient

		public Patient AddNewPatient(Patient patientData)
		{
			var patients = Patients.Where(p => p.ID == patientData.ID);
			if (patients.Any())
				return patients.FirstOrDefault();

			patientData.Status = UserStatus.Active;
			patientData.CreatedAt = DateTime.Now;
			patientData.UpdatedAt = DateTime.Now;
			Patients.Add(patientData);
			return patientData;
		}

		public Patient UpdateExistingPatient(Patient patientData)
		{
			var patients = Patients.Where(p => p.ID == patientData.ID && p.ClientID == patientData.ClientID);
			if (patients.Any() && patientData.ClientID == this.ID)
			{
				Patients.Remove(patients.Single(p => p.ID == patientData.ID && p.ClientID == patientData.ClientID));
				patientData.UpdatedAt = DateTime.Now;
				Patients.Add(patientData);
				return patientData;
			}
			return null;
		}

		public Patient GetExistingPatient(int patientID)
		{
			var patients = Patients.Where(p => p.ID == patientID);
			if (patients.Any())
			{
				return patients.Single(p => p.ID == patientID);
			}
			return null;
		}

		public List<Patient> GetAllExistingPatients()
		{
			return this.Patients.ToList();
		}

		#endregion

		#region booking

		public Booking CreateNewBooking(Booking bookingData)
		{
			var existing = this.Bookings.Where(b => b.ID == bookingData.ID);
			if (existing.Any())
			{
				var message = string.Format("Booking with ID {0} already exists. Use UpdateBooking to update an existing booking", bookingData.ID);
				throw new ArgumentException(message, "bookingData");
			}

			this.Bookings.Add(bookingData);
			return bookingData;
		}

		public Booking UpdateBooking(Booking bookingData)
		{
			if (bookingData == null)
				return null;

			var existing = this.Bookings.Where(b => b.ID == bookingData.ID).SingleOrDefault();
			if (existing != null)
			{
				this.Bookings.Remove(existing);
				this.Bookings.Add(bookingData);
			}

			return null;
		}

		public Booking GetBooking(int bookingID)
		{
			return this.Bookings.Where(b => b.ID == bookingID).SingleOrDefault();
		}

		public List<Booking> GetBookings()
		{
			return null;
			//		return this.Bookings.ToList();
		}

		#endregion

		#region job

		public Job GetJob(int jobID)
		{
			return this.Jobs.Where(b => b.ID == jobID).SingleOrDefault();
		}

		public Job UpdateJob(Job job)
		{
			if (job == null)
				return null;

			job.JobStatus = JobStatus.Updated;
			job.UpdatedAt = DateTime.Now;
			var existing = GetJob(job.ID);
			if (existing != null)
			{
				this.Jobs.Remove(existing);
				this.Jobs.Add(job);
				return job;
			}

			return null;
		}

		public void DeleteJob(int jobID)
		{
			var job = GetJob(jobID);
			this.Jobs.Remove(job);
		}

		#endregion
	}
}
