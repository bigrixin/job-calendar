using System;
using System.Collections.Generic;
using System.Linq;

namespace MyAbilityFirst.Domain
{
	public class Booking
	{

		#region Properties

		public int ID { get; set; }
		public int ClientID { get; private set; }
		public int CareWorkerID { get; private set; }
		public int? JobID { get; private set; }
		public Schedule Schedule { get; private set; }
		public BookingStatus Status { get; private set; }
		public virtual ICollection<CaseNote> CaseNotes { get; set; }
		public virtual ICollection<Rating> Rating { get; set; }
		public virtual ICollection<Replacement> Replacements { get; set; }
		public DateTime CreatedAt { get; private set; }
		public DateTime UpdatedAt { get; private set; }
		public String Message { get; set; }
		private bool CanBeUpdated => !(this.Status == BookingStatus.Cancelled || this.Status == BookingStatus.Completed);
		private bool CanBeAccepted => this.Status == BookingStatus.Requested;
		private bool CanBeCancelled => this.Status != BookingStatus.Completed;
		private bool CanBeCompleted => this.Status == BookingStatus.Accepted;
		private bool CanBeRejected => this.Status == BookingStatus.Requested && CanBeUpdated;
		private bool CanBePended => this.Status == BookingStatus.Accepted;
		private bool CanBeReplaced => this.Status == BookingStatus.Pending;

		#endregion

		#region Ctor

		protected Booking()
		{
			// Required by EF
			this.CaseNotes = new List<CaseNote>();
			this.Rating = new List<Rating>();
		}

		public Booking(int clientID, int careWorkerID, Schedule schedule) : this(clientID, careWorkerID, null, schedule)
		{
		}

		public Booking(int clientID, int careWorkerID, int? jobID, Schedule schedule)
		{
			this.ClientID = clientID;
			this.CareWorkerID = careWorkerID;
			this.JobID = jobID;
			this.Schedule = schedule;
			this.Status = BookingStatus.Requested;
			this.CaseNotes = new List<CaseNote>();
			this.Rating = new List<Rating>();
			var now = DateTime.Now;
			this.CreatedAt = now;
			this.UpdatedAt = now;
		}

		public Booking(int clientID, int careWorkerID, Schedule schedule, string message)
		{
			this.ClientID = clientID;
			this.CareWorkerID = careWorkerID;
			this.Schedule = schedule;
			this.Status = BookingStatus.Requested;
			this.CaseNotes = new List<CaseNote>();
			this.Rating = new List<Rating>();
			this.Message = message;
			var now = DateTime.Now;
			this.CreatedAt = now;
			this.UpdatedAt = now;
		}

		#endregion

		#region Helpers

		public void AddCaseNote(int userId, string note)
		{
			var caseNote = new CaseNote(this.ID, userId, note);
			this.CaseNotes.Add(caseNote);
		}

		public CaseNote GetCaseNote(int caseNoteId)
		{
			return this.CaseNotes.Where(c => c.ID == caseNoteId).SingleOrDefault();
		}

		public void UpdateCaseNote(CaseNote caseNote)
		{
			var item = this.GetCaseNote(caseNote.ID);
			if (item != null)
			{
				this.CaseNotes.Remove(item);
				this.CaseNotes.Add(caseNote);
			}
		}

		public CaseNote DeleteCaseNote(int caseNoteId, int userId)
		{
			var item = this.GetCaseNotesByUserId(userId).Where(c => c.ID == caseNoteId).SingleOrDefault();
			if (item != null)
				this.CaseNotes.Remove(item);

			return item;
		}

		public List<CaseNote> GetCaseNotes()
		{
			return this.CaseNotes.ToList();
		}

		public List<CaseNote> GetCaseNotesByUserId(int userId)
		{
			return this.CaseNotes.Where(c => c.OwnerUserID == userId).ToList();
		}

		public void UpdateSchedule(Schedule newSchedule)
		{
			if (this.Schedule == newSchedule)
				return;

			if (this.CanBeUpdated)
			{
				this.Schedule = newSchedule;
				this.UpdatedAt = DateTime.Now;
			}
		}

		public void UpdateJobID(int jobID)
		{
			if (this.JobID == jobID)
				return;

			if (this.CanBeUpdated)
			{
				this.JobID = jobID;
				this.UpdatedAt = DateTime.Now;
			}
		}

		public bool Cancel()
		{
			if (this.CanBeCancelled)
			{
				this.Status = BookingStatus.Cancelled;
				this.UpdatedAt = DateTime.Now;
				return true;
			}
			return false;
		}

		public bool Accept()
		{
			if (this.CanBeAccepted)
			{
				this.Status = BookingStatus.Accepted;
				this.UpdatedAt = DateTime.Now;
				return true;
			}
			return false;
		}

		public bool Reject()
		{
			if (this.CanBeRejected)
			{
				this.Status = BookingStatus.Rejected;
				this.UpdatedAt = DateTime.Now;
				return true;
			}
			return false;
		}

		public bool Complete()
		{
			if (this.CanBeCompleted)
			{
				this.Status = BookingStatus.Completed;
				this.UpdatedAt = DateTime.Now;
				return true;
			}
			return false;
		}

		public void AddRating(Rating ratingData)
		{
			var rating = new Rating(this.ID);
			rating = ratingData;
			this.Rating.Add(rating);
		}

		public void UpdateRating(Rating ratingData)
		{
			var rating = this.Rating.Where(c => c.ID == ratingData.ID).SingleOrDefault();
			if (rating != null)
			{
				this.Rating.Remove(rating);
				this.Rating.Add(ratingData);
			}
		}

		public Rating GetRating()
		{
			return this.Rating.Where(c => c.BookingID == this.ID).FirstOrDefault();
		}

		public bool Pending()
		{
			if (this.CanBePended)
			{
				this.Status = BookingStatus.Pending;
				this.UpdatedAt = DateTime.Now;
				return true;
			}
			return false;
		}

		public bool Replace()
		{
			if (this.CanBeReplaced)
			{
				this.Status = BookingStatus.Reassigned;
				this.UpdatedAt = DateTime.Now;
				return true;
			}
			return false;
		}

		public Replacement GetReplacement(int replacementID)
		{
			return this.Replacements.Where(r => r.BookingID == this.ID).FirstOrDefault();
		}

		#endregion

	}
}