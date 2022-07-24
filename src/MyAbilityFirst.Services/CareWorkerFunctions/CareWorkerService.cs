using AutoMapper;
using MyAbilityFirst.Domain;
using MyAbilityFirst.Domain.ClientFunctions;
using MyAbilityFirst.Infrastructure;
using MyAbilityFirst.Infrastructure.Data;
using MyAbilityFirst.Infrastructure.Communication;
using MyAbilityFirst.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using MyAbilityFirst.Infrastructure.Auth;
using MyAbilityFirst.Models;

namespace MyAbilityFirst.Services.CareWorkerFunctions
{
	public class CareWorkerService : UserService, ICareWorkerService
	{

		#region Fields

		private readonly IWriteEntities _entities;
		private IMapper _mapper;
		private readonly IEmailService _emailServices;
		private readonly BookingData _bookingData;
		private readonly IPresentationService _presentationService;

		#endregion

		public CareWorkerService(IWriteEntities entities, IMapper mapper, IBroadcastService hub, ILoginService loginServices, IEmailService emailServices, BookingData bookingData, IPresentationService presentationService) : base(entities, mapper, hub, loginServices)
		{
			this._entities = entities;
			this._mapper = mapper;
			this._emailServices = emailServices;
			this._bookingData = bookingData;
			this._presentationService = presentationService;
		}


		#region profile

		public CareWorker RetrieveCareWorker(int careWorkerID)
		{
			return this._entities.Single<CareWorker>(c => c.ID == careWorkerID);
		}

		public CareWorker RetrieveCareWorkerByLoginID(string identityId)
		{
			return this._entities.Single<CareWorker>(c => c.LoginIdentityId == identityId);
		}

		public CareWorker UpdateCareWorker(CareWorker updatedCareWorker)
		{
			// default is UserStatus.Registered, need coordinator to approve change to UserStatus.Active
			updatedCareWorker.Status = UserStatus.Registered;    
			updatedCareWorker.UpdatedAt = DateTime.Now;
			this._entities.Update(updatedCareWorker);
			this._entities.Save();
			return updatedCareWorker;
		}

		public EmploymentHistory CreateEmploymentHistory(int careWorkerID, EmploymentHistory employmentHistoryData)
		{
			CareWorker parentCareWorker = RetrieveCareWorker(careWorkerID);
			if (parentCareWorker != null)
			{
				EmploymentHistory newEmploymentHistory = parentCareWorker.AddNewEmploymentHistory(employmentHistoryData);
				this._entities.Update(parentCareWorker);
				this._entities.Save();
				return newEmploymentHistory;
			}
			return null;
		}

		public EmploymentHistory RetrieveEmploymentHistory(int careWorkerID, int employmentHistoryID)
		{
			CareWorker parentCareWorker = RetrieveCareWorker(careWorkerID);
			if (parentCareWorker != null)
			{
				return parentCareWorker.GetExistingEmploymentHistory(employmentHistoryID);
			}
			return null;
		}

		public List<EmploymentHistory> RetrieveAllEmploymentHistories(int careWorkerID)
		{
			CareWorker parentCareWorker = RetrieveCareWorker(careWorkerID);
			if (parentCareWorker != null)
			{
				return parentCareWorker.GetAllExistingEmploymentHistories();
			}
			return null;
		}

		public EmploymentHistory UpdateEmploymentHistory(int careWorkerID, EmploymentHistory employmentHistoryData)
		{
			CareWorker parentCareWorker = RetrieveCareWorker(careWorkerID);
			if (parentCareWorker != null)
			{
				EmploymentHistory existingEmploymentHistory = RetrieveEmploymentHistory(careWorkerID, employmentHistoryData.ID);
				_mapper.Map(employmentHistoryData, existingEmploymentHistory);
				parentCareWorker.UpdateExistingEmploymentHistory(existingEmploymentHistory);
				this._entities.Update(existingEmploymentHistory);
				this._entities.Save();
				return existingEmploymentHistory;
			}
			return null;
		}

		public bool DeleteEmploymentHistory(int careWorkerID, int employmentHistoryID)
		{
			CareWorker parentCareWorker = RetrieveCareWorker(careWorkerID);
			if (parentCareWorker != null)
			{
				EmploymentHistory employmentHistory = parentCareWorker.RemoveEmploymentHistory(employmentHistoryID);
				this._entities.Delete(employmentHistory);
				this._entities.Save();
				return true;
			}
			return false;
		}

		public EmploymentFormalEducation CreateEmploymentFormalEducation(int careWorkerID, EmploymentFormalEducation employmentFormalEducationData)
		{
			CareWorker parentCareWorker = RetrieveCareWorker(careWorkerID);
			if (parentCareWorker != null)
			{
				EmploymentFormalEducation newEmploymentFormalEducation = parentCareWorker.AddNewEmploymentFormalEducation(employmentFormalEducationData);
				this._entities.Update(parentCareWorker);
				this._entities.Save();
				return newEmploymentFormalEducation;
			}
			return null;
		}

		public EmploymentFormalEducation RetrieveEmploymentFormalEducation(int careWorkerID, int employmentFormalEducationID)
		{
			CareWorker parentCareWorker = RetrieveCareWorker(careWorkerID);
			if (parentCareWorker != null)
			{
				return parentCareWorker.GetExistingEmploymentFormalEducation(employmentFormalEducationID);
			}
			return null;
		}

		public List<EmploymentFormalEducation> RetrieveAllEmploymentFormalEducations(int careWorkerID)
		{
			CareWorker parentCareWorker = RetrieveCareWorker(careWorkerID);
			if (parentCareWorker != null)
			{
				return parentCareWorker.GetAllExistingEmploymentFormalEducation();
			}
			return null;
		}

		public EmploymentFormalEducation UpdateEmploymentFormalEducation(int careWorkerID, EmploymentFormalEducation employmentFormalEducationData)
		{
			CareWorker parentCareWorker = RetrieveCareWorker(careWorkerID);
			if (parentCareWorker != null)
			{
				EmploymentFormalEducation existingEmploymentFormalEducation = RetrieveEmploymentFormalEducation(careWorkerID, employmentFormalEducationData.ID);
				_mapper.Map(employmentFormalEducationData, existingEmploymentFormalEducation);
				parentCareWorker.UpdateExistingEmploymentFormalEducation(existingEmploymentFormalEducation);
				this._entities.Update(existingEmploymentFormalEducation);
				this._entities.Save();
				return existingEmploymentFormalEducation;
			}
			return null;
		}

		public bool DeleteEmploymentFormalEducation(int careWorkerID, int employmentFormalEducationID)
		{
			CareWorker parentCareWorker = RetrieveCareWorker(careWorkerID);
			if (parentCareWorker != null)
			{
				EmploymentFormalEducation employmentFormalEducation = parentCareWorker.RemoveEmploymentFormalEducation(employmentFormalEducationID);
				this._entities.Delete(employmentFormalEducation);
				this._entities.Save();
				return true;
			}
			return false;
		}

		public EmploymentReference CreateEmploymentReference(int careWorkerID, EmploymentReference employmentReferenceData)
		{
			CareWorker parentCareWorker = RetrieveCareWorker(careWorkerID);
			if (parentCareWorker != null)
			{
				EmploymentReference newEmploymentReference = parentCareWorker.AddNewEmploymentReference(employmentReferenceData);
				this._entities.Update(parentCareWorker);
				this._entities.Save();
				return newEmploymentReference;
			}
			return null;
		}

		public EmploymentReference RetrieveEmploymentReference(int careWorkerID, int employmentReferenceID)
		{
			CareWorker parentCareWorker = RetrieveCareWorker(careWorkerID);
			if (parentCareWorker != null)
			{
				return parentCareWorker.GetExistingEmploymentReference(employmentReferenceID);
			}
			return null;
		}

		public List<EmploymentReference> RetrieveAllEmploymentReferences(int careWorkerID)
		{
			CareWorker parentCareWorker = RetrieveCareWorker(careWorkerID);
			if (parentCareWorker != null)
			{
				return parentCareWorker.GetAllExistingEmploymentReference();
			}
			return null;
		}

		public EmploymentReference UpdateEmploymentReference(int careWorkerID, EmploymentReference employmentReferenceData)
		{
			CareWorker parentCareWorker = RetrieveCareWorker(careWorkerID);
			if (parentCareWorker != null)
			{
				EmploymentReference existingEmploymentReference = RetrieveEmploymentReference(careWorkerID, employmentReferenceData.ID);
				_mapper.Map(employmentReferenceData, existingEmploymentReference);
				parentCareWorker.UpdateExistingEmploymentReference(existingEmploymentReference);
				this._entities.Update(existingEmploymentReference);
				this._entities.Save();
				return existingEmploymentReference;
			}
			return null;
		}

		public bool DeleteEmploymentReference(int careWorkerID, int employmentReferenceID)
		{
			CareWorker parentCareWorker = RetrieveCareWorker(careWorkerID);
			if (parentCareWorker != null)
			{
				EmploymentReference employmentReference = parentCareWorker.RemoveEmploymentReference(employmentReferenceID);
				this._entities.Delete(employmentReference);
				this._entities.Save();
				return true;
			}
			return false;
		}

		public EmploymentAchievement CreateEmploymentAchievement(int careWorkerID, EmploymentAchievement employmentAchievementData)
		{
			CareWorker parentCareWorker = RetrieveCareWorker(careWorkerID);
			if (parentCareWorker != null)
			{
				EmploymentAchievement newEmploymentAchievement = parentCareWorker.AddNewEmploymentAchievement(employmentAchievementData);
				this._entities.Update(parentCareWorker);
				this._entities.Save();
				return newEmploymentAchievement;
			}
			return null;
		}

		public EmploymentAchievement RetrieveEmploymentAchievement(int careWorkerID, int employmentAchievementID)
		{
			CareWorker parentCareWorker = RetrieveCareWorker(careWorkerID);
			if (parentCareWorker != null)
			{
				return parentCareWorker.GetExistingEmploymentAchievement(employmentAchievementID);
			}
			return null;
		}

		public List<EmploymentAchievement> RetrieveAllEmploymentAchievements(int careWorkerID)
		{
			CareWorker parentCareWorker = RetrieveCareWorker(careWorkerID);
			if (parentCareWorker != null)
			{
				return parentCareWorker.GetAllExistingEmploymentAchievement();
			}
			return null;
		}

		public EmploymentAchievement UpdateEmploymentAchievement(int careWorkerID, EmploymentAchievement employmentAchievementData)
		{
			CareWorker parentCareWorker = RetrieveCareWorker(careWorkerID);
			if (parentCareWorker != null)
			{
				EmploymentAchievement existingEmploymentAchievement = RetrieveEmploymentAchievement(careWorkerID, employmentAchievementData.ID);
				_mapper.Map(employmentAchievementData, existingEmploymentAchievement);
				parentCareWorker.UpdateExistingEmploymentAchievement(existingEmploymentAchievement);
				this._entities.Update(existingEmploymentAchievement);
				this._entities.Save();
				return existingEmploymentAchievement;
			}
			return null;
		}

		public bool DeleteEmploymentAchievement(int careWorkerID, int employmentAchievementID)
		{
			CareWorker parentCareWorker = RetrieveCareWorker(careWorkerID);
			if (parentCareWorker != null)
			{
				EmploymentAchievement employmentAchievement = parentCareWorker.RemoveEmploymentAchievement(employmentAchievementID);
				this._entities.Delete(employmentAchievement);
				this._entities.Save();
				return true;
			}
			return false;
		}

		public Availability CreateAvailability(int careWorkerID, Availability availabilityData)
		{
			CareWorker parentCareWorker = RetrieveCareWorker(careWorkerID);
			if (parentCareWorker != null)
			{
				Availability newAvailability = parentCareWorker.AddNewAvailability(availabilityData);
				this._entities.Update(parentCareWorker);
				this._entities.Save();
				return newAvailability;
			}
			return null;
		}

		public Availability RetrieveAvailability(int careWorkerID, int availabilityID)
		{
			CareWorker parentCareWorker = RetrieveCareWorker(careWorkerID);
			if (parentCareWorker != null)
			{
				return parentCareWorker.GetExistingAvailability(availabilityID);
			}
			return null;
		}

		public List<Availability> RetrieveAllAvailabilites(int careWorkerID)
		{
			CareWorker parentCareWorker = RetrieveCareWorker(careWorkerID);
			if (parentCareWorker != null)
			{
				return parentCareWorker.GetAllExistingAvailability();
			}
			return null;
		}

		public Availability UpdateAvailability(int careWorkerID, Availability availabilityData)
		{
			CareWorker parentCareWorker = RetrieveCareWorker(careWorkerID);
			if (parentCareWorker != null)
			{
				Availability existingAvailability = RetrieveAvailability(careWorkerID, availabilityData.ID);
				_mapper.Map(availabilityData, existingAvailability);
				parentCareWorker.UpdateExistingAvailability(existingAvailability);
				this._entities.Update(existingAvailability);
				this._entities.Save();
				return existingAvailability;
			}
			return null;
		}

		public bool DeleteAvailability(int careWorkerID, int availabilityID)
		{
			CareWorker parentCareWorker = RetrieveCareWorker(careWorkerID);
			if (parentCareWorker != null)
			{
				Availability employmentAvailability = parentCareWorker.RemoveAvailability(availabilityID);
				this._entities.Delete(employmentAvailability);
				this._entities.Save();
				return true;
			}
			return false;
		}

		public List<EmploymentHistory> ReplaceAllEmploymentHistories(int careWorkerID, IEnumerable<EmploymentHistory> employmentHistories)
		{
			List<EmploymentHistory> existingItems = RetrieveAllEmploymentHistories(careWorkerID) ?? new List<EmploymentHistory>();
			employmentHistories = employmentHistories ?? new List<EmploymentHistory>();
			IEnumerable<EmploymentHistory> actionableObjects = employmentHistories.Except(existingItems, new EmploymentHistoryComparer());
			foreach (EmploymentHistory obj in actionableObjects)
			{
				CreateEmploymentHistory(careWorkerID, obj);
			}
			actionableObjects = employmentHistories.Intersect(existingItems, new EmploymentHistoryComparer());
			foreach (EmploymentHistory obj in actionableObjects)
			{
				UpdateEmploymentHistory(careWorkerID, obj);
			}
			actionableObjects = existingItems.Except(employmentHistories, new EmploymentHistoryComparer());
			foreach (EmploymentHistory obj in actionableObjects)
			{
				DeleteEmploymentHistory(careWorkerID, obj.ID);
			}
			return RetrieveAllEmploymentHistories(careWorkerID);
		}

		public List<EmploymentFormalEducation> ReplaceAllEmploymentFormalEducations(int careWorkerID, IEnumerable<EmploymentFormalEducation> employmentFormalEducations)
		{
			List<EmploymentFormalEducation> existingItems = RetrieveAllEmploymentFormalEducations(careWorkerID) ?? new List<EmploymentFormalEducation>();
			employmentFormalEducations = employmentFormalEducations ?? new List<EmploymentFormalEducation>();
			IEnumerable<EmploymentFormalEducation> actionableObjects = employmentFormalEducations.Except(existingItems, new EmploymentFormalEducationComparer());
			foreach (EmploymentFormalEducation obj in actionableObjects)
			{
				CreateEmploymentFormalEducation(careWorkerID, obj);
			}
			actionableObjects = employmentFormalEducations.Intersect(existingItems, new EmploymentFormalEducationComparer());
			foreach (EmploymentFormalEducation obj in actionableObjects)
			{
				UpdateEmploymentFormalEducation(careWorkerID, obj);
			}
			actionableObjects = existingItems.Except(employmentFormalEducations, new EmploymentFormalEducationComparer());
			foreach (EmploymentFormalEducation obj in actionableObjects)
			{
				DeleteEmploymentFormalEducation(careWorkerID, obj.ID);
			}
			return RetrieveAllEmploymentFormalEducations(careWorkerID);
		}

		public List<EmploymentReference> ReplaceAllEmploymentReferences(int careWorkerID, IEnumerable<EmploymentReference> employmentReferences)
		{
			List<EmploymentReference> existingItems = RetrieveAllEmploymentReferences(careWorkerID) ?? new List<EmploymentReference>();
			employmentReferences = employmentReferences ?? new List<EmploymentReference>();
			IEnumerable<EmploymentReference> actionableObjects = employmentReferences.Except(existingItems, new EmploymentReferenceComparer());
			foreach (EmploymentReference obj in actionableObjects)
			{
				CreateEmploymentReference(careWorkerID, obj);
			}
			actionableObjects = employmentReferences.Intersect(existingItems, new EmploymentReferenceComparer());
			foreach (EmploymentReference obj in actionableObjects)
			{
				UpdateEmploymentReference(careWorkerID, obj);
			}
			actionableObjects = existingItems.Except(employmentReferences, new EmploymentReferenceComparer());
			foreach (EmploymentReference obj in actionableObjects)
			{
				DeleteEmploymentReference(careWorkerID, obj.ID);
			}
			return RetrieveAllEmploymentReferences(careWorkerID);
		}

		public List<EmploymentAchievement> ReplaceAllEmploymentAchievements(int careWorkerID, IEnumerable<EmploymentAchievement> employmentAchievements)
		{
			List<EmploymentAchievement> existingItems = RetrieveAllEmploymentAchievements(careWorkerID) ?? new List<EmploymentAchievement>();
			employmentAchievements = employmentAchievements ?? new List<EmploymentAchievement>();
			IEnumerable<EmploymentAchievement> actionableObjects = employmentAchievements.Except(existingItems, new EmploymentAchievementComparer());
			foreach (EmploymentAchievement obj in actionableObjects)
			{
				CreateEmploymentAchievement(careWorkerID, obj);
			}
			actionableObjects = employmentAchievements.Intersect(existingItems, new EmploymentAchievementComparer());
			foreach (EmploymentAchievement obj in actionableObjects)
			{
				UpdateEmploymentAchievement(careWorkerID, obj);
			}
			actionableObjects = existingItems.Except(employmentAchievements, new EmploymentAchievementComparer());
			foreach (EmploymentAchievement obj in actionableObjects)
			{
				DeleteEmploymentAchievement(careWorkerID, obj.ID);
			}
			return RetrieveAllEmploymentAchievements(careWorkerID);
		}

		public List<Availability> ReplaceAllAvailabilities(int careWorkerID, IEnumerable<Availability> availabilities)
		{
			List<Availability> existingItems = RetrieveAllAvailabilites(careWorkerID) ?? new List<Availability>();
			availabilities = availabilities ?? new List<Availability>();
			IEnumerable<Availability> actionableObjects = availabilities.Except(existingItems, new AvailabilityComparer());
			foreach (Availability obj in actionableObjects)
			{
				CreateAvailability(careWorkerID, obj);
			}
			actionableObjects = availabilities.Intersect(existingItems, new AvailabilityComparer());
			foreach (Availability obj in actionableObjects)
			{
				UpdateAvailability(careWorkerID, obj);
			}
			actionableObjects = existingItems.Except(availabilities, new AvailabilityComparer());
			foreach (Availability obj in actionableObjects)
			{
				DeleteAvailability(careWorkerID, obj.ID);
			}
			return RetrieveAllAvailabilites(careWorkerID);
		}

		#endregion

		#region booking

		public UpdateBookingViewModel GetBookingViewModel(int careWorkerID, int bookingID)
		{
			var careWorker = this._entities.Single<CareWorker>(a => a.ID == careWorkerID);
			if (careWorker == null)
				throw new ArgumentNullException("careWorker");

			var booking = careWorker.GetBooking(bookingID);
			if (booking == null)
				throw new ArgumentNullException("booking");

			var client = this._entities.Get<Client>(a => a.ID == booking.ClientID).SingleOrDefault();
			if (client == null)
				throw new ArgumentNullException("client");

			var vm = _mapper.Map<Booking, UpdateBookingViewModel>(booking);
			vm.CaseNotes = vm.CaseNotes.AsEnumerable().Reverse().ToList();
			vm.CareWorkerFirstName = careWorker.FirstName;
			vm.ClientFirstName = client.FirstName;
			vm.ClientID = client.ID;
			vm.CareWorkerID = careWorker.ID;
			vm.PreviousBookingID = this._presentationService.GetPreviousBookingID(bookingID);
			return vm;
		}

		public IEnumerable<UpdateBookingViewModel> GetListOfBookingsViewModel(int careWorkerID)
		{
			var careWorker = this._entities.Single<CareWorker>(a => a.ID == careWorkerID);
			if (careWorker == null)
				throw new ArgumentNullException("careWorker");

			return this._bookingData.GetBookingVMListByCareWorker(careWorker.ID);
		}

		public void UpdateBooking(int careWorkerId, UpdateBookingViewModel bookingDetails)
		{
			var careWorker = this._entities.Single<CareWorker>(u => u.ID == careWorkerId);
			if (careWorker == null)
				throw new ArgumentNullException("careWorker");

			Booking booking = careWorker.GetBooking(bookingDetails.BookingID);
			if (booking == null)
				throw new ArgumentNullException("booking");

			// update case note
			string note = "";
			if (bookingDetails.Note == null)
				note = $"{careWorker.FirstName} has updated his booking content: From {bookingDetails.Start} to {bookingDetails.End}, {booking.Message}";
			else
			{
				note = $"{careWorker.FirstName} has added a note: {bookingDetails.Note}";
				booking.AddCaseNote(careWorker.ID, note);
			}

			// save changes to database
			this._entities.Update(careWorker);
			this._entities.Save();

			// send alert email to the other booking party to let him/her know the booking has changed
			this._emailServices.SendBookingUpdatedByCareWorkerEmail(booking, note);
		}

		public void AcceptBooking(int careWorkerID, int bookingID)
		{
			if (careWorkerID < 1)
				throw new ArgumentOutOfRangeException("Value must be an int greater than 1", "careWorkerID");

			if (bookingID < 1)
				throw new ArgumentOutOfRangeException("Value must be an int greater than 1", "bookingID");

			var careWorker = this._entities.Single<CareWorker>(c => c.ID == careWorkerID);
			if (careWorker == null)
				throw new ArgumentNullException("careWorker");

			var booking = careWorker.GetBooking(bookingID);
			if (booking == null)
				throw new ArgumentNullException("booking");

			// accept booking
			if (booking.Accept())
			{
				// update case note
				var note = $"{careWorker.FirstName} has accepted your help request.";
				booking.AddCaseNote(careWorker.ID, note);

				// save changes to database
				this._entities.Update(careWorker);
				this._entities.Save();

				// send alert email to client
				this._emailServices.SendBookingAcceptedEmail(booking, note);
			}
		}

		public void RejectBooking(int careWorkerID, int bookingID)
		{
			if (careWorkerID < 1)
				throw new ArgumentOutOfRangeException("Value must be an int greater than 1", "careWorkerID");

			if (bookingID < 1)
				throw new ArgumentOutOfRangeException("Value must be an int greater than 1", "bookingID");

			var careWorker = this._entities.Single<CareWorker>(c => c.ID == careWorkerID);
			if (careWorker == null)
				throw new ArgumentNullException("careWorker");

			var booking = careWorker.GetBooking(bookingID);
			if (booking == null)
				throw new ArgumentNullException("booking");

			// reject booking
			if (booking.Reject())
			{
				// update case note
				var note = $"{careWorker.FirstName} has rejected your help request.";
				booking.AddCaseNote(careWorker.ID, note);

				this._entities.Update(careWorker);
				this._entities.Save();

				// send alert email to client
				this._emailServices.SendBookingRejectedEmail(booking, note);
			}
		}


		public ReplacementViewModel GetReplacementVM(int carerID, int bookingID)
		{
			ReplacementViewModel vm = new ReplacementViewModel();
			var carer = this._entities.Single<CareWorker>(a => a.ID == carerID);
			var booking = carer.GetBooking(bookingID);
			if (booking == null)
				throw new ArgumentNullException("booking");
			vm.BookingID = booking.ID;
			vm.ClientID = booking.ClientID;
			vm.CareWorkerID = carerID;
			return vm;
		}

		public void CreateReplacement(ReplacementViewModel vm)
		{
			var carer = this._entities.Single<CareWorker>(a => a.ID == vm.CareWorkerID);
			var booking = carer.GetBooking(vm.BookingID);
			if (booking == null)
				throw new ArgumentNullException("booking");

			if (booking.Pending())
			{
				// update case note
				var note = $"{carer.FirstName} has submitted a replacement request.";
				booking.AddCaseNote(carer.ID, note);

				// create repacement request
				Replacement replacement = _mapper.Map<ReplacementViewModel, Replacement>(vm);
				replacement.CreatedAt = DateTime.Now;
				replacement.UpdatedAt = DateTime.Now;
				this._entities.Create(replacement);
				this._entities.Save();

				// send alert email to client
				this._emailServices.SendBookingReplacementRequestEmail(booking, note);
			}
		}

		public UpdateBookingViewModel GetPreviousBookingViewModel(int bookingID)
		{
			var booking = this._entities.Single<Booking>(a => a.ID == bookingID);
			if (booking == null)
				throw new ArgumentNullException("booking");

			var client = this._entities.Get<Client>(a => a.ID == booking.ClientID).SingleOrDefault();
			if (client == null)
				throw new ArgumentNullException("client");

			var careWorker = this._entities.Single<CareWorker>(a => a.ID == booking.CareWorkerID);

			var vm = _mapper.Map<Booking, UpdateBookingViewModel>(booking);
			vm.CaseNotes = vm.CaseNotes.AsEnumerable().Reverse().ToList();
			vm.CareWorkerFirstName = careWorker.FirstName;
			vm.ClientFirstName = client.FirstName;
			vm.ClientID = client.ID;
			vm.CareWorkerID = careWorker.ID;
			vm.PreviousBookingID = this._presentationService.GetPreviousBookingID(bookingID);
			return vm;
		}

		#endregion

		#region rating

		public void AddOverallRating(int careWorkerID, Rating rating)
		{
			var careWorker = this._entities.Single<CareWorker>(c => c.ID == careWorkerID);
			if (careWorker == null)
				throw new ArgumentNullException("careWorker");
			careWorker.AddOverallRating(rating.OverallScore);
		}

		public void UpdateOverallRating(int careWorkerID, double oldRating, Rating rating)
		{
			var careWorker = this._entities.Single<CareWorker>(c => c.ID == careWorkerID);
			if (careWorker == null)
				throw new ArgumentNullException("careWorker");
			careWorker.UpdateOverallRating(oldRating, rating.OverallScore);
		}

		#endregion

	}
}
