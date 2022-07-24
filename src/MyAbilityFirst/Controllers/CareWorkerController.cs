using AutoMapper;
using MyAbilityFirst.Domain;
using MyAbilityFirst.Domain.ClientFunctions;
using MyAbilityFirst.Models;
using MyAbilityFirst.Services.CareWorkerFunctions;
using MyAbilityFirst.Services.Common;
using System.Collections.Generic;
using System.Web.Mvc;
using PagedList;
using System;
using System.Configuration;
using System.Web;

namespace MyAbilityFirst.Controllers
{
	[Authorize(Roles = "CareWorker")]
	public class CareWorkerController : Controller
	{

		#region Fields

		private readonly IMapper _mapper;
		private readonly ICareWorkerService _careWorkerServices;
		private readonly IPresentationService _categoryServices;
		private readonly IUploadService _uploadServices;

		#endregion

		#region Ctor

		public CareWorkerController(ICareWorkerService careWorkerServices, IMapper mapper, IPresentationService categoryServices, IUploadService uploadServices)
		{
			this._careWorkerServices = careWorkerServices;
			this._mapper = mapper;
			this._categoryServices = categoryServices;
			this._uploadServices = uploadServices;
		}

		#endregion

		#region Actions

		[HttpGet, Route("careworker/editprofile")]
		public ActionResult EditProfile()
		{
			CareWorkerDetailsViewModel vm = mapCareWorkerToVM();
			return View(vm);
		}

		[HttpPost, Route("careworker/editprofile")]
		[ValidateAntiForgeryToken]
		public ActionResult EditProfile(CareWorkerDetailsViewModel vm)
		{
			if (ModelState.IsValid)
			{
				CareWorker updatedCareWorker = mapVMToCareWorker(vm);
				var careWorkerID = this.GetLoggedInUser().ID;
				_careWorkerServices.ReplaceAllEmploymentHistories(careWorkerID, vm.EmploymentHistories);
				_careWorkerServices.ReplaceAllEmploymentFormalEducations(careWorkerID, vm.EmploymentFormalEducations);
				_careWorkerServices.ReplaceAllEmploymentReferences(careWorkerID, vm.EmploymentReferences);
				_careWorkerServices.ReplaceAllEmploymentAchievements(careWorkerID, vm.EmploymentAchievements);
				_careWorkerServices.ReplaceAllAvailabilities(careWorkerID, vm.Availabilities);
				_careWorkerServices.ReplaceAllUserSubCategories(careWorkerID, vm.PostedSubCategoryIDs, new List<UserSubcategory>());
				_careWorkerServices.UpdateCareWorker(updatedCareWorker);
			}
			return RedirectToAction("MyAccount");
		}

		[HttpGet, Route("careworker/myaccount")]
		public ActionResult MyAccount()
		{
			CareWorkerDetailsViewModel vm = mapCareWorkerToVM();
			return View(vm);
		}

		[HttpPost]
		public ActionResult NewEmploymentHistory(CareWorkerDetailsViewModel vm)
		{
			EmploymentHistory newEmploymentHistory = new EmploymentHistory();
			newEmploymentHistory.CareWorkerID = this.GetLoggedInUser().ID;
			vm.EmploymentHistories = vm.EmploymentHistories ?? new List<EmploymentHistory>();
			vm.EmploymentHistories.Add(newEmploymentHistory);
			return PartialView("_NewEmploymentHistory", vm);
		}

		[HttpPost]
		public ActionResult NewEmploymentFormalEducation(CareWorkerDetailsViewModel vm)
		{
			EmploymentFormalEducation newEmploymentFormalEducation = new EmploymentFormalEducation();
			newEmploymentFormalEducation.CareWorkerID = this.GetLoggedInUser().ID;
			vm.EmploymentFormalEducations = vm.EmploymentFormalEducations ?? new List<EmploymentFormalEducation>();
			vm.FormalEducationDropDownList = _categoryServices.GetSubCategorySelectList("FormalEducation");
			vm.EmploymentFormalEducations.Add(newEmploymentFormalEducation);
			return PartialView("_NewEmploymentFormalEducation", vm);
		}

		[HttpPost]
		public ActionResult NewEmploymentReference(CareWorkerDetailsViewModel vm)
		{
			EmploymentReference newEmploymentReference = new EmploymentReference();
			newEmploymentReference.CareWorkerID = this.GetLoggedInUser().ID;
			vm.EmploymentReferences = vm.EmploymentReferences ?? new List<EmploymentReference>();
			vm.EmploymentReferences.Add(newEmploymentReference);
			return PartialView("_NewEmploymentReference", vm);
		}

		[HttpPost]
		public ActionResult NewEmploymentAchievement(CareWorkerDetailsViewModel vm)
		{
			EmploymentAchievement newEmploymentAchievement = new EmploymentAchievement();
			newEmploymentAchievement.CareWorkerID = this.GetLoggedInUser().ID;
			vm.EmploymentAchievements = vm.EmploymentAchievements ?? new List<EmploymentAchievement>();
			vm.EmploymentAchievements.Add(newEmploymentAchievement);
			return PartialView("_NewEmploymentAchievement", vm);
		}

		#endregion

		#region Actions-bookings

		[HttpGet, Route("careworker/bookings")]
		public ActionResult Bookings(int? page)
		{
			var carer = this.GetLoggedInUser();
			var bookings = this._careWorkerServices.GetListOfBookingsViewModel(carer.ID);

			int pageIndex = 1;
			int pageSize = 10;
			int pageNumber = (page ?? 1);
			pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

			return View(bookings.ToPagedList(pageNumber, pageSize));
		}

		[HttpGet, Route("careworker/booking/{bookingID:int}")]
		public ActionResult Booking(int bookingID, int? page)
		{
			var carer = this.GetLoggedInUser();
			if (bookingID == 0)
				return View("Error");
			ViewBag.UserId = carer.ID;
			var bookingVM = this._careWorkerServices.GetBookingViewModel(carer.ID, bookingID);
			if (bookingVM == null)
				return View("Error");

			int pageIndex = 1;
			int pageSize = 10;
			int pageNumber = (page ?? 1);
			pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
			ViewBag.Notes = bookingVM.CaseNotes.ToPagedList(pageNumber, pageSize);
			return View(bookingVM);
		}

		[HttpPost, Route("careworker/booking/{bookingID:int}")]
		public ActionResult Booking(UpdateBookingViewModel vm)
		{
			var carer = _careWorkerServices.RetrieveCareWorker(this.GetLoggedInUser().ID);
			var booking = carer.GetBooking(vm.BookingID);
			if (booking != null && !string.IsNullOrEmpty(vm.Note.Trim()))
				this._careWorkerServices.UpdateBooking(carer.ID, vm);

			return RedirectToAction("Booking", new { bookingID = vm.BookingID });
		}

		[HttpGet, Route("careworker/booking/accept/{bookingID:int}")]
		public ActionResult AcceptBooking(int bookingID)
		{
			var carer = _careWorkerServices.RetrieveCareWorker(this.GetLoggedInUser().ID);
			var booking = carer.GetBooking(bookingID);
			if (booking != null)
			{
				this._careWorkerServices.AcceptBooking(carer.ID, bookingID);
				return RedirectToAction("Booking", new { bookingID = bookingID });
			}
			return RedirectToAction("Bookings");
		}

		[HttpGet, Route("careworker/booking/reject/{bookingID:int}")]
		public ActionResult RejectBooking(int bookingID)
		{
			var carer = _careWorkerServices.RetrieveCareWorker(this.GetLoggedInUser().ID);
			var booking = carer.GetBooking(bookingID);
			if (booking != null)
			{
				this._careWorkerServices.RejectBooking(carer.ID, bookingID);
				return RedirectToAction("Booking", new { bookingID = bookingID });
			}
			return RedirectToAction("Bookings");
		}

		[HttpGet]
		public ActionResult RatingDetails(int bookingID)
		{
			var carer = _careWorkerServices.RetrieveCareWorker(this.GetLoggedInUser().ID);
			var booking = carer.GetBooking(bookingID);
			if (booking == null)
				return View("Error");

			var rating = booking.GetRating();
			var vm = new RatingViewModel();
			vm = _mapper.Map<Rating, RatingViewModel>(rating);

			return View(vm);
		}
		#endregion

		#region Actions-Replacement

		[HttpGet, Route("careworker/replacement/{bookingID:int}")]
		public ActionResult Replacement(int bookingID)
		{
			var carer = this.GetLoggedInUser() as CareWorker;
			ReplacementViewModel vm = this._careWorkerServices.GetReplacementVM(carer.ID, bookingID);
			return View(vm);
		}

		[HttpPost, Route("careworker/replacement/{bookingID:int}")]
		public ActionResult Replacement(ReplacementViewModel vm)
		{
			var carer = this.GetLoggedInUser() as CareWorker;
		  this._careWorkerServices.CreateReplacement(vm);
			return RedirectToAction("Booking", new { bookingID = vm.BookingID });
		}

		[HttpGet, Route("careworker/previousbooking/{bookingID:int}")]
		public ActionResult PreviousBooking(int bookingID)
		{
			var carer = this.GetLoggedInUser() as CareWorker;
			var bookingVM = this._careWorkerServices.GetPreviousBookingViewModel(bookingID);
			return View(bookingVM);
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

		private CareWorkerDetailsViewModel mapCareWorkerToVM()
		{
			CareWorkerDetailsViewModel vm = new CareWorkerDetailsViewModel();
			_mapper.Map<CareWorker, CareWorkerDetailsViewModel>(_careWorkerServices.RetrieveCareWorker(this.GetLoggedInUser().ID), vm);
			vm.EmploymentHistories = vm.EmploymentHistories ?? new List<EmploymentHistory>();
			vm.EmploymentFormalEducations = vm.EmploymentFormalEducations ?? new List<EmploymentFormalEducation>();
			vm.EmploymentReferences = vm.EmploymentReferences ?? new List<EmploymentReference>();
			vm.EmploymentAchievements = vm.EmploymentAchievements ?? new List<EmploymentAchievement>();
			vm.Availabilities = vm.Availabilities ?? new List<Availability>();
			return vm;
		}

		private CareWorker mapVMToCareWorker(CareWorkerDetailsViewModel vm)
		{
			return _mapper.Map(vm, _careWorkerServices.RetrieveCareWorker(this.GetLoggedInUser().ID));
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
