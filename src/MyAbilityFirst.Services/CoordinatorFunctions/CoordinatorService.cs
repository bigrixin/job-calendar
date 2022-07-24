using AutoMapper;
using MyAbilityFirst.Domain;
using MyAbilityFirst.Domain.ClientFunctions;
using MyAbilityFirst.Infrastructure;
using MyAbilityFirst.Infrastructure.Auth;
using MyAbilityFirst.Infrastructure.Communication;
using MyAbilityFirst.Infrastructure.Data;
using MyAbilityFirst.Models;
using MyAbilityFirst.Services.ClientFunctions;
using MyAbilityFirst.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MyAbilityFirst.Services.CoordinatorFunctions
{
	public class CoordinatorService : UserService, ICoordinatorService
	{

		#region Fields

		private readonly IWriteEntities _entities;
		private IMapper _mapper;
		private readonly BookingData _bookingData;
		private readonly IClientService _clientServices;
		private readonly IEmailService _emailServices;

		#endregion


		#region Ctor

		public CoordinatorService(IWriteEntities entities, IMapper mapper, IEmailService emailServices, BookingData bookingData, IClientService clientService) : base(entities, mapper)
		{
			this._entities = entities;
			this._mapper = mapper;
			this._emailServices = emailServices;
			this._bookingData = bookingData;
			this._clientServices = clientService;
		}

		#endregion

		#region profile

		public CoordinatorDetailsViewModel GetCoordinatorVM(Coordinator currentCoordinator)
		{
			CoordinatorDetailsViewModel model = new CoordinatorDetailsViewModel();
			model = _mapper.Map<Coordinator, CoordinatorDetailsViewModel>(currentCoordinator);
			return model;
		}


		public void UpdateProfile(Coordinator updatedCoordinator)
		{
			updatedCoordinator.Status = UserStatus.Active;
			updatedCoordinator.UpdatedAt = DateTime.Now;
			this._entities.Update(updatedCoordinator);
			this._entities.Save();
		}

		#endregion

		#region review-approval

		public void ApproveRating(int coordinatorID, int ratingID)
		{
			Rating currentRating = this._entities.Single<Rating>(a => a.ID == ratingID);
			currentRating.Status = RatingStatus.Approved;
			currentRating.ApprovedDate = DateTime.Now;
			currentRating.CoordinatorID = coordinatorID;
			var booking = this._entities.Single<Booking>(b => b.ID == currentRating.BookingID);
			var careWorker = this._entities.Single<CareWorker>(c => c.ID == booking.CareWorkerID);
			careWorker.AddOverallRating(currentRating.OverallScore);

			this._entities.Update(careWorker);
			this._entities.Save();
		}
 
		public void ApproveCareWorker(int coordinatorID, int careWorkerID)
		{
			var careWorker = this._entities.Single<CareWorker>(c => c.ID == careWorkerID);
			if (careWorker == null)
				throw new ArgumentNullException("careWorker");
			careWorker.ApprovedDate = DateTime.Now;
			careWorker.CoordinatorID = coordinatorID;
			careWorker.Status = UserStatus.Active;

			this._entities.Update(careWorker);
			this._entities.Save();
		}


		public ReplacementViewModel GetReplacementVM(int coordinatorID, int replacementID)
		{
			ReplacementViewModel model = new ReplacementViewModel();
			Replacement replacement = this._entities.Single<Replacement>(r => r.ID == replacementID);
			model = _mapper.Map<Replacement, ReplacementViewModel>(replacement);
			model.Shortlist = this._bookingData.GetShortlistForReplacement(coordinatorID, replacement.CareWorkerID)
												 .Select(s => new SelectListItem
												 {
													 Value = s.CareWorkerID.ToString(),
													 Text = s.CareWorkerFirstName
												 });
			return model;
		}

		public void Replacement(int coordinatorID, ReplacementViewModel vm)
		{
			var booking = this._entities.Single<Booking>(a => a.ID == vm.BookingID);
			if (booking == null)
				throw new ArgumentNullException("booking");
			var carer = this._entities.Single<CareWorker>(a => a.ID == vm.CareWorkerID);
			var newCarer = this._entities.Single<CareWorker>(a => a.ID == vm.ReplacedCareWorkerID);
			var note = "";

			if (booking.Replace() && carer!=null && newCarer!=null)
			{
				// create a new booking that same as old booking
				var clientID = booking.ClientID;
				var careWorkerID = newCarer.ID;
			 //				var schedule = new Schedule(booking.Schedule.Duration.Start, booking.Schedule.Duration.End);
				var message = "*** The Booking is from the Booking #" + booking.ID + " *** \n";
	//			var newBooking = this._clientServices.CreateNewBooking(clientID, (int)careWorkerID, schedule, message + booking.Message);

				// update case note
				note = $"Coordinator #{coordinatorID} has added a note: the carer {carer.FirstName} was replaced by {newCarer.FirstName}";
				booking.AddCaseNote(coordinatorID, note);

				note = $"Coordinator #{coordinatorID} has added a note: {message}";
	//			newBooking.AddCaseNote(coordinatorID, note);

				Replacement replacement = _mapper.Map<ReplacementViewModel, Replacement>(vm);
				replacement.CoordinatorID = coordinatorID;
//				replacement.ReplacedBookingID = newBooking.ID;
				replacement.UpdatedAt = DateTime.Now;

				this._entities.Update(replacement);
				this._entities.Save();

				// send alert email to client
				this._emailServices.SendBookingReplacementToClientEmail(booking, note);

				// send alert email to previous carer
				note = $"{carer.FirstName} has a approved replacement request.";
				this._emailServices.SendBookingReplacementToPreviousCarerEmail(booking, note);

			}
		}

		#endregion

	}
}
