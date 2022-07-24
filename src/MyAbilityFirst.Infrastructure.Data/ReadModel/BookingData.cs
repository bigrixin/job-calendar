using MyAbilityFirst.Domain;
using MyAbilityFirst.Domain.ClientFunctions;
using MyAbilityFirst.Models;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;

namespace MyAbilityFirst.Infrastructure.Data
{
	public class BookingData
	{

		#region Fields

		public readonly MyAbilityFirstDbContext _context;

		#endregion

		#region Ctor

		public BookingData(MyAbilityFirstDbContext context)
		{
			this._context = context;
		}

		#endregion

		#region Helpers

		public IEnumerable<NewBookingShortlistViewModel> GetShortlist(int ownerUserID)
		{
			var shortlists =
				from c in this._context.Clients
				join s in this._context.Shortlists on c.ID equals s.OwnerUserID
				join cw in this._context.CareWorkers on s.SelectedUserID equals cw.ID
				where c.ID == ownerUserID
				select new NewBookingShortlistViewModel
				{
					CareWorkerID = cw.ID,
					CareWorkerFirstName = cw.FirstName,
					CareWorkerLastName = cw.LastName
				};

			return shortlists;
		}

		public List<UpdateBookingViewModel> GetBookingVMListByCareWorker(int careWorkerID)
		{
			var vmList =
				from cw in this._context.CareWorkers
				join b in this._context.Bookings on cw.ID equals b.CareWorkerID
				join c in this._context.Clients on b.ClientID equals c.ID
				where cw.ID == careWorkerID
				orderby (b.UpdatedAt) descending
				select new UpdateBookingViewModel
				{
					BookingID = b.ID,
					OwnerUserID = b.ClientID,
					CareWorkerID = b.CareWorkerID,
					ClientFirstName = c.FirstName,
					CareWorkerFirstName = cw.FirstName,
					Schedule = b.Schedule,
					Message = b.Message,
					Status = b.Status,
					CaseNotes = b.CaseNotes.ToList(),
					Rating = b.Rating.ToList()
				};

			return vmList.ToList();
		}

		public List<UpdateBookingViewModel> GetBookingVMListByClient(int clientID)
		{
			var vmList =
				from c in this._context.Clients
				join b in this._context.Bookings on c.ID equals b.ClientID
				join cw in this._context.CareWorkers on b.CareWorkerID equals cw.ID
				where c.ID == clientID
				orderby (b.UpdatedAt) descending
				select new UpdateBookingViewModel
				{
					BookingID = b.ID,
					OwnerUserID = b.ClientID,
					CareWorkerID = b.CareWorkerID,
					ClientFirstName = c.FirstName,
					CareWorkerFirstName = cw.FirstName,
					Schedule = b.Schedule,
					Message = b.Message,
					Status = b.Status,
					CaseNotes = b.CaseNotes.ToList(),
					Rating = b.Rating.ToList(),
					Distance = (DbGeography.FromText("POINT(" + c.Address.Longitude.ToString() + " " + c.Address.Latitude.ToString() + ")")
								.Distance(DbGeography.FromText("POINT(" + cw.Address.Longitude.ToString() + " " + cw.Address.Latitude.ToString()
						 + ")")) / 1000).Value.ToString()
				};

			return vmList.ToList();
		}


		public List<BookingRatingsViewModel> GetBookingRatingsVMList(int coordinatorID)
		{
			var vmList =
				from co in this._context.Coordinators
				join cw in this._context.CareWorkers on
				 new { OrganisationId = co.OrganisationId, CoordinatorID = co.ID == coordinatorID } equals
				 new { OrganisationId = cw.OrganisationId, CoordinatorID = true }
				join b in this._context.Bookings on
					new { CareworkerID = cw.ID, BookingCompleted = true } equals
					new { CareworkerID = b.CareWorkerID, BookingCompleted = b.Status == BookingStatus.Completed }
				join c in this._context.Clients on b.ClientID equals c.ID
				join r in this._context.Ratings on
					new { BookingID = b.ID, RatingCompleted = true } equals
					new { BookingID = r.ID, RatingCompleted = r.Status == RatingStatus.New || r.Status == RatingStatus.Update }
				orderby (b.UpdatedAt) descending
				select new BookingRatingsViewModel
				{
					BookingID = b.ID,
					ClientID = b.ClientID,
					CareWorkerID = b.CareWorkerID,
					RatingID = r.ID,
					ClientFirstName = c.FirstName,
					CareWorkerFirstName = cw.FirstName,
					OverallScore = r.OverallScore,
					CreatedAt = b.CreatedAt,
					Approved = false
				};

			return vmList.ToList();
		}


		public List<ReplacementViewModel> GetReplacementsVMList(int coordinatorID)
		{
			var vmList =
			from co in this._context.Coordinators
			join cw in this._context.CareWorkers on
			 new { OrganisationId = co.OrganisationId, CoordinatorID = co.ID == coordinatorID } equals
			 new { OrganisationId = cw.OrganisationId, CoordinatorID = true }
			join b in this._context.Bookings on cw.ID equals b.CareWorkerID
			join c in this._context.Clients on b.ClientID equals c.ID
			join r in this._context.Replacements on b.ID equals r.BookingID
			where b.Status == BookingStatus.Pending && r.CoordinatorID == null
			orderby (b.UpdatedAt) descending
			select new ReplacementViewModel
			{
				ID = r.ID,
				BookingID = b.ID,
				ClientID = b.ClientID,
				CareWorkerID = b.CareWorkerID,
				ClientFirstName = c.FirstName,
				CareWorkerFirstName = cw.FirstName,
				CommentByCareWorker = r.CommentByCareWorker,
				CreatedAt=r.CreatedAt
			};

			return vmList.ToList();
		}

		public IEnumerable<NewBookingShortlistViewModel> GetShortlistForReplacement(int ownerUserID, int careWorkerID)
		{
			var shortlists =
				from c in this._context.Coordinators
				join s in this._context.Shortlists on c.ID equals s.OwnerUserID
				join cw in this._context.CareWorkers on s.SelectedUserID equals cw.ID
				where c.ID == ownerUserID && cw.ID!=careWorkerID
				select new NewBookingShortlistViewModel
				{
					CareWorkerID = cw.ID,
					CareWorkerFirstName = cw.FirstName,
					CareWorkerLastName = cw.LastName
				};

			return shortlists;
		}



		public List<CareWorkerReviewViewModel> GetCareWorkerVMList(int coordinatorID)
		{
			var vmList =
							from co in this._context.Coordinators
							join cw in this._context.CareWorkers on
							 new { OrganisationId = co.OrganisationId, CareWorkerStatus = true } equals
									 new { OrganisationId = cw.OrganisationId, CareWorkerStatus = cw.Status == UserStatus.Registered }
							where co.ID == coordinatorID
							select new CareWorkerReviewViewModel
							{
								CareWorkerID = cw.ID,
								FirstName = cw.FirstName,
								LastName = cw.LastName,
								Suburb = cw.Address.Suburb,
								Email = cw.Email,
								Phone = cw.Phone,
								CreatedDate = cw.CreatedAt,
								Status = cw.Status
							};

			return vmList.ToList();
		}

		#endregion
	}
}
