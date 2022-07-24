using AutoMapper;
using MyAbilityFirst.Domain;
using MyAbilityFirst.Domain.ClientFunctions;
using MyAbilityFirst.Models;
using MyAbilityFirst.Services.ClientFunctions;
using MyAbilityFirst.Services.Common;
using NWebsec.Mvc.HttpHeaders.Csp;
using PagedList;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyAbilityFirst.Controllers
{
	[Authorize(Roles = "Client")]
	public class ClientController : Controller
	{

		#region Fields

		private readonly IMapper _mapper;
		private readonly IClientService _clientServices;
		private readonly IPresentationService _categoryServices;
		private readonly IUploadService _uploadServices;
		#endregion

		#region Ctor

		public ClientController(IMapper mapper, IClientService clientServices, IPresentationService categoryServices, IUploadService uploadServices)
		{
			this._mapper = mapper;
			this._clientServices = clientServices;
			this._categoryServices = categoryServices;
			this._uploadServices = uploadServices;
		}

		#endregion

		#region Actions

		[HttpGet, Route("client/profile/edit")]
		public ActionResult EditProfile()
		{
			ClientDetailsViewModel vm = mapClientToVM();
			return View(vm);
		}

		[HttpPost, Route("client/profile/edit")]
		[ValidateAntiForgeryToken]
		public ActionResult EditProfile(ClientDetailsViewModel vm)
		{
			if (ModelState.IsValid)
			{
				Client updatedClient = mapVMToClient(vm);
				_clientServices.UpdateClient(updatedClient);

				_clientServices.ReplaceAllUserSubCategories(updatedClient.ID, vm.PostedSubCategoryIDs, new List<UserSubcategory>());
			}
			return RedirectToAction("MyAccount/");
		}

		[HttpGet, Route("client/myaccount")]
		public ActionResult MyAccount()
		{
			ClientDetailsViewModel vm = mapClientToVM();
			return View(vm);
		}

		#endregion

		#region Actions-bookings

		[HttpGet, Route("client/booking/new")]
		public ActionResult NewBooking()
		{
			var client = this.GetLoggedInUser() as Client;

			var vm = new NewBookingViewModel
			{
				Shortlist = this._clientServices.GetShortlist(client.ID)
																				.Select(s => new SelectListItem
																				{
																					Value = s.CareWorkerID.ToString(),
																					Text = s.CareWorkerFirstName
																				}),
				Start = DateTime.Today,
				End = DateTime.Today.AddDays(1)
			};

			return View("Booking/Create", vm);
		}

		[HttpPost, Route("client/booking/new")]
		public ActionResult NewBooking(NewBookingViewModel vm)
		{
			if (ModelState.IsValid)
			{
				var client = this.GetLoggedInUser();
				var clientID = client.ID;
				var careWorkerID = vm.CareWorkerID;
		//		var schedule = new Schedule(vm.Start, vm.End);
				var message = vm.Message;
				//			var booking = this._clientServices.CreateNewBooking(clientID, careWorkerID, schedule, message);

				//			return RedirectToAction("Booking", new { bookingID = booking.ID });
				return null;
			}

			return View("Booking/Create", vm);
		}

		[HttpGet, Route("client/booking/edit/{bookingID:int}")]
		public ActionResult EditBooking(int bookingID)
		{
			var client = _clientServices.RetrieveClient(this.GetLoggedInUser().ID);
			var booking = client.GetBooking(bookingID);
			if (booking == null)
				return View("Error");

			var vm = _mapper.Map<Booking, UpdateBookingViewModel>(booking);
			return View("Booking/Edit", vm);
		}

		[HttpPost, Route("client/booking/edit/{bookingID:int}")]
		[ValidateAntiForgeryToken]
		public ActionResult EditBooking(UpdateBookingViewModel vm)
		{
			if (ModelState.IsValid)
			{
				var client = _clientServices.RetrieveClient(this.GetLoggedInUser().ID);
				var booking = client.GetBooking(vm.BookingID);
				if (booking != null)
					this._clientServices.UpdateBooking(client.ID, vm);

				return RedirectToAction("Booking", new { vm.BookingID });
			}

			return View("Booking/Edit", vm);
		}

		[HttpGet, Route("client/booking/cancel/{bookingID:int}")]
		public ActionResult CancelBooking(int bookingID)
		{
			var client = this.GetLoggedInUser();
			this._clientServices.CancelBooking(client.ID, bookingID);

			return RedirectToAction("BookingHistory");
		}

		[HttpGet, Route("client/booking/complete/{bookingID:int}")]
		public ActionResult CompleteBooking(int bookingID)
		{
			var client = _clientServices.RetrieveClient(this.GetLoggedInUser().ID);
			var booking = client.GetBooking(bookingID);
			if (booking == null)
				return View("Error");

			this._clientServices.CompleteBooking(client.ID, bookingID);
			// here for rating
			return RedirectToAction("Rating", new { bookingID });
		}

		[HttpGet, Route("client/booking/{bookingID:int}")]
		public ActionResult Booking(int bookingId, int? page)
		{
			var client = _clientServices.RetrieveClient(this.GetLoggedInUser().ID);
			var booking = client.GetBooking(bookingId);
			if (booking == null)
				return View("Error");

			ViewBag.UserId = client.ID;
			var bookingVM = this._clientServices.GetBookingViewModel(booking.ID, client.ID);

			int pageIndex = 1;
			int pageSize = 10;
			int pageNumber = (page ?? 1);
			pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

			ViewBag.Notes = bookingVM.CaseNotes.ToPagedList(pageNumber, pageSize);

			return View(bookingVM);
		}

		[HttpPost, Route("client/booking/{bookingID:int}")]
		public ActionResult Booking(UpdateBookingViewModel vm)
		{
			var client = _clientServices.RetrieveClient(this.GetLoggedInUser().ID);
			var booking = client.GetBooking(vm.BookingID);
			if (booking != null && !string.IsNullOrEmpty(vm.Note.Trim()))
				this._clientServices.UpdateBooking(client.ID, vm);

			return RedirectToAction("Booking", new { vm.BookingID });
		}

		[HttpGet]
		public ActionResult BookingHistory(int? page)
		{
			var client = this.GetLoggedInUser();
			var bookings = this._clientServices.GetListOfBookingsViewModel(client.ID);

			int pageIndex = 1;
			int pageSize = 10;
			int pageNumber = (page ?? 1);
			pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
			return View(bookings.ToPagedList(pageNumber, pageSize));
		}

		[HttpGet, Route("client/previousbooking/{bookingID:int}")]
		public ActionResult PreviousBooking(int bookingID)
		{
			var client = this.GetLoggedInUser() as Client;
			var booking = client.GetBooking(bookingID);
			if (booking == null)
				return View("Error");

			ViewBag.UserId = client.ID;
			var bookingVM = this._clientServices.GetBookingViewModel(booking.ID, client.ID);
			return View(bookingVM);
		}

		#endregion

		#region Actions-rating

		[HttpGet]
		public ActionResult Rating(int bookingID)
		{
			var vm = new RatingViewModel();
			return View(vm);
		}

		[HttpPost]
		public ActionResult Rating(RatingViewModel vm)
		{
			var client = this.GetLoggedInUser();
			this._clientServices.AddRating(client.ID, vm);
			return RedirectToAction("RatingDetails", new { vm.BookingID });
		}

		[HttpGet]
		public ActionResult UpdateRating(int bookingID)
		{
			var client = this.GetLoggedInUser();
			var vm = this._clientServices.GetRatingVM(client.ID, bookingID);
			vm.OldOverallScore = vm.OverallScore;
			return View(vm);
		}

		[HttpPost]
		public ActionResult UpdateRating(RatingViewModel vm)
		{
			var client = this.GetLoggedInUser();
			this._clientServices.UpdateRating(client.ID, vm.OldOverallScore, vm);
			return RedirectToAction("RatingDetails", new { vm.BookingID });
		}

		[HttpGet]
		public ActionResult RatingDetails(int bookingID)
		{
			var client = this.GetLoggedInUser();
			var vm = this._clientServices.GetRatingVM(client.ID, bookingID);
			return View(vm);
		}

		#endregion

		#region Actions-uploadfile

		//upload file to Azure storage blob
		[HttpPost]
		public virtual ActionResult UploadFileToAzure()
		{
			string path = ConfigurationManager.AppSettings["uploadAzurePath_Careworker"];
			HttpPostedFileBase file = Request.Files[0];
			string url = this._uploadServices.UploadToAzureStorage(file, path);
			return attachmentProcess(url);
		}

		[HttpDelete]
		public virtual ActionResult DeleteFileFromAzure(string fileName)
		{
			string path = ConfigurationManager.AppSettings["uploadAzurePath_Careworker"];
			if (this._uploadServices.DeleteFromAzureStorage(fileName, path))
				return Json(new { message = "The file has delete !" }, "text/html");
			else
				return Json(new { message = "Error" }, "text/html");
		}

		#endregion

		#region Helpers

		private ClientDetailsViewModel mapClientToVM()
		{
			ClientDetailsViewModel vm = new ClientDetailsViewModel();
			var client = _clientServices.RetrieveClient(this.GetLoggedInUser().ID);
			return _mapper.Map<Client, ClientDetailsViewModel>(client);
		}

		private Client mapVMToClient(ClientDetailsViewModel vm)
		{
			return _mapper.Map(vm, (Client)this.GetLoggedInUser());
		}

		private JsonResult attachmentProcess(string url)
		{
			bool isUploaded = false;
			if (url != null)
			{
				isUploaded = true;
				string message = "100% complete";

				return Json(new
				{
					statusCode = 200,
					status = "File uploaded.",
					file = url,
					isUploaded = isUploaded,
					message = message
				}, "text/html");

			}
			else
			{
				string message = "Error";
				return Json(new
				{
					statusCode = 500,
					status = "Error uploading image.",
					file = string.Empty,
					isUploaded = isUploaded,
					message = message
				}, "text/html");
			}
		}

		#endregion

	}
}
