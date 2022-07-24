using System;
using System.Collections.Generic;
using System.Linq;

namespace MyAbilityFirst.Domain
{
	public class CareWorker : User
	{

		#region Properties

		public virtual ICollection<EmploymentHistory> EmploymentHistories { get; set; }
		public virtual ICollection<EmploymentFormalEducation> EmploymentFormalEducations { get; set; }
		public virtual ICollection<EmploymentReference> EmploymentReferences { get; set; }
		public virtual ICollection<EmploymentAchievement> EmploymentAchievements { get; set; }
		public virtual ICollection<Availability> Availabilities { get; set; }
		public virtual ICollection<Booking> Bookings { get; set; }
		public virtual ICollection<Job> Jobs { get; set; }
		public int GenderPreferenceID { get; set; }
		public int ProvisionLocationSuburbID { get; set; }
		public int LocationCoverageRadius { get; set; }
		public int FirstLanguageID { get; set; }
		public int SecondLanguageID { get; set; }
		public int CultureID { get; set; }
		public int ReligionID { get; set; }
		public int PersonalityID { get; set; }
		public string Description { get; set; }
		public double TotalRating { get; set; }
		public int RatingCount { get; set; }
  	public int OrganisationId { get; set; }
  	public DateTime? StartingDate { get; set; }
		public int? CoordinatorID { get; set; }
		public DateTime? ApprovedDate { get; set; }

		#endregion

		#region Ctor

		protected CareWorker()
		{
			// required by EF
			this.EmploymentHistories = new List<EmploymentHistory>();
			this.EmploymentFormalEducations = new List<EmploymentFormalEducation>();
			this.EmploymentReferences = new List<EmploymentReference>();
			this.EmploymentAchievements = new List<EmploymentAchievement>();
			this.Availabilities = new List<Availability>();
			this.Bookings = new List<Booking>();
		}

		public CareWorker(string aspNetIdentity) : base(aspNetIdentity)
		{
			this.EmploymentHistories = new List<EmploymentHistory>();
			this.EmploymentFormalEducations = new List<EmploymentFormalEducation>();
			this.EmploymentReferences = new List<EmploymentReference>();
			this.EmploymentAchievements = new List<EmploymentAchievement>();
			this.Availabilities = new List<Availability>();
			this.Bookings = new List<Booking>();
		}

		#endregion

		public EmploymentHistory AddNewEmploymentHistory(EmploymentHistory employmentHistoryData)
		{
			var employmentHistories = EmploymentHistories.Where(h => h.ID == employmentHistoryData.ID && h.CareWorkerID == employmentHistoryData.CareWorkerID);
			if (employmentHistories.Any() || employmentHistoryData.CareWorkerID != this.ID)
			{
				return null;
			}
			this.EmploymentHistories.Add(employmentHistoryData);
			return employmentHistoryData;
		}

		public EmploymentHistory GetExistingEmploymentHistory(int employmentHistoryID)
		{
			var employmentHistories = EmploymentHistories.Where(h => h.ID == employmentHistoryID);
			if (employmentHistories.Any())
			{
				return employmentHistories.Single(h => h.ID == employmentHistoryID);
			}
			return null;
		}

		public List<EmploymentHistory> GetAllExistingEmploymentHistories()
		{
			return this.EmploymentHistories.ToList();
		}

		public EmploymentHistory UpdateExistingEmploymentHistory(EmploymentHistory employmentHistoryData)
		{
			var employmentHistories = EmploymentHistories.Where(h => h.ID == employmentHistoryData.ID && h.CareWorkerID == employmentHistoryData.CareWorkerID);
			if (employmentHistories.Any() && employmentHistoryData.CareWorkerID == this.ID)
			{
				EmploymentHistories.Remove(employmentHistories.Single(h => h.ID == employmentHistoryData.ID && h.CareWorkerID == employmentHistoryData.CareWorkerID));
				EmploymentHistories.Add(employmentHistoryData);
				return employmentHistoryData;
			}
			return null;
		}

		public EmploymentHistory RemoveEmploymentHistory(int employmentHistoryID)
		{
			EmploymentHistory existingEmploymentHistory = GetExistingEmploymentHistory(employmentHistoryID);
			if (existingEmploymentHistory != null && this.EmploymentHistories.Remove(existingEmploymentHistory))
			{
				return existingEmploymentHistory;
			}
			return null;
		}

		public EmploymentFormalEducation AddNewEmploymentFormalEducation(EmploymentFormalEducation employmentFormalEducationData)
		{
			var employmentFormalEducations = EmploymentFormalEducations.Where(h => h.ID == employmentFormalEducationData.ID && h.CareWorkerID == employmentFormalEducationData.CareWorkerID);
			if (employmentFormalEducations.Any() || employmentFormalEducationData.CareWorkerID != this.ID)
			{
				return null;
			}
			this.EmploymentFormalEducations.Add(employmentFormalEducationData);
			return employmentFormalEducationData;
		}

		public EmploymentFormalEducation GetExistingEmploymentFormalEducation(int employmentFormalEducationID)
		{
			var employmentFormalEducations = EmploymentFormalEducations.Where(h => h.ID == employmentFormalEducationID);
			if (employmentFormalEducations.Any())
			{
				return employmentFormalEducations.Single(f => f.ID == employmentFormalEducationID);
			}
			return null;
		}

		public List<EmploymentFormalEducation> GetAllExistingEmploymentFormalEducation()
		{
			return this.EmploymentFormalEducations.ToList();
		}

		public EmploymentFormalEducation UpdateExistingEmploymentFormalEducation(EmploymentFormalEducation employmentFormalEducationData)
		{
			var employmentFormalEducations = EmploymentFormalEducations.Where(h => h.ID == employmentFormalEducationData.ID && h.CareWorkerID == employmentFormalEducationData.CareWorkerID);
			if (employmentFormalEducations.Any() && employmentFormalEducationData.CareWorkerID == this.ID)
			{
				EmploymentFormalEducations.Remove(employmentFormalEducations.Single(h => h.ID == employmentFormalEducationData.ID && h.CareWorkerID == employmentFormalEducationData.CareWorkerID));
				EmploymentFormalEducations.Add(employmentFormalEducationData);
				return employmentFormalEducationData;
			}
			return null;
		}

		public EmploymentFormalEducation RemoveEmploymentFormalEducation(int employmentFormalEducationID)
		{
			EmploymentFormalEducation existingEmploymentFormalEducation = GetExistingEmploymentFormalEducation(employmentFormalEducationID);
			if (existingEmploymentFormalEducation != null && this.EmploymentFormalEducations.Remove(existingEmploymentFormalEducation))
			{
				return existingEmploymentFormalEducation;
			}
			return null;
		}

		public EmploymentReference AddNewEmploymentReference(EmploymentReference employmentReferenceData)
		{
			var employmentReferences = EmploymentReferences.Where(h => h.ID == employmentReferenceData.ID && h.CareWorkerID == employmentReferenceData.CareWorkerID);
			if (employmentReferences.Any() || employmentReferenceData.CareWorkerID != this.ID)
			{
				return null;
			}
			this.EmploymentReferences.Add(employmentReferenceData);
			return employmentReferenceData;
		}

		public EmploymentReference GetExistingEmploymentReference(int employmentReferenceID)
		{
			var employmentReferences = EmploymentReferences.Where(h => h.ID == employmentReferenceID);
			if (employmentReferences.Any())
			{
				return employmentReferences.Single(f => f.ID == employmentReferenceID);
			}
			return null;
		}

		public List<EmploymentReference> GetAllExistingEmploymentReference()
		{
			return this.EmploymentReferences.ToList();
		}

		public EmploymentReference UpdateExistingEmploymentReference(EmploymentReference employmentReferenceData)
		{
			var employmentReferences = EmploymentReferences.Where(h => h.ID == employmentReferenceData.ID && h.CareWorkerID == employmentReferenceData.CareWorkerID);
			if (employmentReferences.Any() && employmentReferenceData.CareWorkerID == this.ID)
			{
				EmploymentReferences.Remove(employmentReferences.Single(h => h.ID == employmentReferenceData.ID && h.CareWorkerID == employmentReferenceData.CareWorkerID));
				EmploymentReferences.Add(employmentReferenceData);
				return employmentReferenceData;
			}
			return null;
		}

		public EmploymentReference RemoveEmploymentReference(int employmentReferenceID)
		{
			EmploymentReference existingEmploymentReference = GetExistingEmploymentReference(employmentReferenceID);
			if (existingEmploymentReference != null && this.EmploymentReferences.Remove(existingEmploymentReference))
			{
				return existingEmploymentReference;
			}
			return null;
		}

		public EmploymentAchievement AddNewEmploymentAchievement(EmploymentAchievement employmentAchievementData)
		{
			var employmentAchievement = EmploymentAchievements.Where(h => h.ID == employmentAchievementData.ID && h.CareWorkerID == employmentAchievementData.CareWorkerID);
			if (employmentAchievement.Any() || employmentAchievementData.CareWorkerID != this.ID)
			{
				return null;
			}
			this.EmploymentAchievements.Add(employmentAchievementData);
			return employmentAchievementData;
		}

		public EmploymentAchievement GetExistingEmploymentAchievement(int employmentAchievementID)
		{
			var employmentAchievement = EmploymentAchievements.Where(h => h.ID == employmentAchievementID);
			if (employmentAchievement.Any())
			{
				return employmentAchievement.Single(f => f.ID == employmentAchievementID);
			}
			return null;
		}

		public List<EmploymentAchievement> GetAllExistingEmploymentAchievement()
		{
			return this.EmploymentAchievements.ToList();
		}

		public EmploymentAchievement UpdateExistingEmploymentAchievement(EmploymentAchievement employmentAchievementData)
		{
			var employmentAchievement = EmploymentAchievements.Where(h => h.ID == employmentAchievementData.ID && h.CareWorkerID == employmentAchievementData.CareWorkerID);
			if (employmentAchievement.Any() && employmentAchievementData.CareWorkerID == this.ID)
			{
				EmploymentAchievements.Remove(employmentAchievement.Single(h => h.ID == employmentAchievementData.ID && h.CareWorkerID == employmentAchievementData.CareWorkerID));
				EmploymentAchievements.Add(employmentAchievementData);
				return employmentAchievementData;
			}
			return null;
		}

		public EmploymentAchievement RemoveEmploymentAchievement(int employmentAchievementID)
		{
			EmploymentAchievement existingEmploymentAchievement = GetExistingEmploymentAchievement(employmentAchievementID);
			if (existingEmploymentAchievement != null && this.EmploymentAchievements.Remove(existingEmploymentAchievement))
			{
				return existingEmploymentAchievement;
			}
			return null;
		}

		public Availability AddNewAvailability(Availability availabilityData)
		{
			var availability = Availabilities.Where(h => h.ID == availabilityData.ID && h.CareWorkerID == availabilityData.CareWorkerID);
			if (availability.Any() || availabilityData.CareWorkerID != this.ID)
			{
				return null;
			}
			this.Availabilities.Add(availabilityData);
			return availabilityData;
		}

		public Availability GetExistingAvailability(int availabilityID)
		{
			var availability = Availabilities.Where(h => h.ID == availabilityID);
			if (availability.Any())
			{
				return Availabilities.Single(f => f.ID == availabilityID);
			}
			return null;
		}

		public List<Availability> GetAllExistingAvailability()
		{
			return this.Availabilities.ToList();
		}

		public Availability UpdateExistingAvailability(Availability availabilityData)
		{
			var availability = Availabilities.Where(h => h.ID == availabilityData.ID && h.CareWorkerID == availabilityData.CareWorkerID);
			if (availability.Any() && availabilityData.CareWorkerID == this.ID)
			{
				Availabilities.Remove(availability.Single(h => h.ID == availabilityData.ID && h.CareWorkerID == availabilityData.CareWorkerID));
				Availabilities.Add(availabilityData);
				return availabilityData;
			}
			return null;
		}

		public Availability RemoveAvailability(int availabilityID)
		{
			Availability existingAvailability = GetExistingAvailability(availabilityID);
			if (existingAvailability != null && this.Availabilities.Remove(existingAvailability))
			{
				return existingAvailability;
			}
			return null;
		}

		public Booking GetBooking(int bookingID)
		{
			return this.Bookings.Where(b => b.ID == bookingID).SingleOrDefault();
		}

		public List<Booking> GetBookings()
		{
			return this.Bookings.ToList();
		}

		public double AddOverallRating(double rating)
		{
			this.TotalRating = TotalRating + rating;
			this.RatingCount++;
			return Math.Round((this.TotalRating) / (this.RatingCount), 1);
		}

		public double UpdateOverallRating(double oldRating, double newRating)
		{
			this.TotalRating = TotalRating - oldRating + newRating;
			return Math.Round((this.TotalRating) / (this.RatingCount), 1);
		}
	}
}