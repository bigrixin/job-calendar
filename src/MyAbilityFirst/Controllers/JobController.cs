using AutoMapper;
using MyAbilityFirst.Domain;
using MyAbilityFirst.Infrastructure;
using MyAbilityFirst.Models;
using MyAbilityFirst.Services.ClientFunctions;
using MyAbilityFirst.Services.Common;
using NWebsec.Mvc.HttpHeaders.Csp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;

using System.Security.Cryptography.X509Certificates;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using MyAbilityFirst.Services.CareWorkerFunctions;

namespace MyAbilityFirst.Controllers
{
	[Authorize]
	[Csp(Enabled = false)]
	public class JobController : Controller
	{

		#region Fields

		private readonly IWriteEntities _entities;
		private readonly Services.ClientFunctions.IClientService _clientServices;
		private readonly IJobService  _jobServices;
		private readonly IUploadService _uploadServices;
		private readonly IPresentationService _presentationService;
		private readonly IMapper _mapper;
		private readonly ICalendarService _calendarServices;

		#endregion

		#region Ctor

		public JobController(IWriteEntities entities, Services.ClientFunctions.IClientService clientServices, IUploadService uploadServices, IPresentationService presentationService, IMapper mapper, ICalendarService calendarServices, IJobService jobServices)
		{
			this._entities = entities;
			this._clientServices = clientServices;
			this._uploadServices = uploadServices;
			this._presentationService = presentationService;
			this._mapper = mapper;
			this._calendarServices = calendarServices;
			this._jobServices=jobServices;
		}

		#endregion

		#region Actions

		[HttpGet, Route("job")]
		public ActionResult Index()
		{
			var client = _clientServices.RetrieveClient(this.GetLoggedInUser().ID);
			var jobs = client.Jobs;
			var jobsVM = new List<JobViewModel>();
			jobs.ToList().ForEach(j =>
			{
				var vm = mapJobToJobViewModel(j);
				jobsVM.Add(vm);
			});

			return View(jobsVM);
		}

		[HttpGet, Route("job/create")]
		public ActionResult Create()
		{
			JobViewModel model = new JobViewModel();
			var client = this.GetLoggedInUser();
			model.GenderDropDownList = _presentationService.GetSubCategorySelectList("Gender");
			model.ServiceDropDownList = _presentationService.GetSubCategorySelectList("JobService");
			model.PatientDropDownList = _presentationService.GetPatientSelectList(client.ID);
			ViewBag.PathUpload = "/Job/UploadFileToAzure";
			ViewBag.PathDelete = "/Job/DeleteFileFromAzure";
			return View(model);
		}

		[HttpPost, Route("job/create")]
		[ValidateAntiForgeryToken]
		public ActionResult Create(JobViewModel model, IEnumerable<HttpPostedFileBase> files)
		{
			if (ModelState.IsValid)
			{
				var client = this.GetLoggedInUser();
				Job newJob = this._clientServices.PostNewJob(client.ID, model);
				return RedirectToAction("Details/" + newJob.ID.ToString());
			}

			return View(model);
		}

		[HttpGet, Route("job/edit/{id:int}")]
		public ActionResult Edit(int? id)
		{
			if (id == null)
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

			var client = _clientServices.RetrieveClient(this.GetLoggedInUser().ID);
			var job = client.Jobs.SingleOrDefault(j => j.ID == id);

			if (job == null)
				return RedirectToAction("MyAccount", "Client");

			JobViewModel model = mapJobToJobViewModel(job);
			ViewBag.pictureURL = model.PictureURL;
			return View(model);
		}

		[HttpPost, Route("job/edit/{id:int}")]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(JobViewModel model, IEnumerable<HttpPostedFileBase> files)
		{
			if (ModelState.IsValid)
			{
				var client = this.GetLoggedInUser();
				this._clientServices.EditJob(client.ID, model);
				return RedirectToAction("Index", "Job");
			}

			return View(model);
		}

		[HttpGet, Route("job/details/{id:int}")]
		public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			var client = _clientServices.RetrieveClient(this.GetLoggedInUser().ID);
			Job job = client.Jobs.SingleOrDefault(j => j.ID == id);

			if (job == null)
				return RedirectToAction("MyAccount", "Client");

			JobViewModel model = mapJobToJobViewModel(job);

			return View(model);
		}

		[Authorize(Roles = "Client, Admin")]
		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			var client = this.GetLoggedInUser() as Client;
			Job job = client.Jobs.Where(j => j.ID == id).SingleOrDefault();

			if (job == null)
				return RedirectToAction("MyAccount", "Client");

			JobViewModel model = mapJobToJobViewModel(job);

			return View(model);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id)
		{
			var client = this.GetLoggedInUser();
			this._clientServices.DeleteJob(client.ID, id);
			return RedirectToAction("Index");
		}


		#endregion

		#region Job-actions

		[HttpGet, Route("jobs")]
		public ActionResult Jobs(string currentFilter, string searchString, int? page)
		{
			int PageSize = 10;
			int pageIndex = 1;
			int pageNumber = (page ?? 1);
			ViewBag.CurrentFilter = searchString;
			if (searchString != null)
				page = 1;
			else
				searchString = currentFilter;
			pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
			var jobsVM = this._jobServices.GetPublicJobsViewModelList();
			if (!String.IsNullOrEmpty(searchString))
				jobsVM = jobsVM.Where(s => s.Title.ToLower().Contains(searchString.ToLower())
									 || s.Description.ToLower().Contains(searchString.ToLower())).ToList();

			return View(jobsVM.ToPagedList(pageNumber, PageSize));
		}

		[HttpGet, Route("details")]
		public ActionResult Details(int id)
		{
			JobsViewModel vm = new JobsViewModel();
			vm = this._jobServices.GetJobViewModel(id);

		//	ViewBag.visitors = getVisitors();
			return View(vm);
		}

		//--------------------------------------------------------------------------------
		public int getVisitors()
		{
			//	string[] scopes = new string[] { AnalyticsService.Scope.AnalyticsReadonly };
			// Environment.CurrentDirectory+@"\App_Data\MyAbilityFirst-GoogleKey.p12";
			var keyFilePath = @"C:\Steven_Doc\angry-chicken2\src\MyAbilityFirst\App_Data\MyAbilityFirst-GoogleKey.p12";

			var serviceAccountEmail = @"myabilityfirst-1487808941446@appspot.gserviceaccount.com";
			//loading the Key file
			var certificate = new X509Certificate2(keyFilePath, "notasecret", X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable);
			var credential = new ServiceAccountCredential(new ServiceAccountCredential.Initializer(serviceAccountEmail)
			{
				Scopes = new[] { Google.Apis.Analytics.v3.AnalyticsService.Scope.AnalyticsReadonly }
			}.FromCertificate(certificate));

			var service = new Google.Apis.Analytics.v3.AnalyticsService(new BaseClientService.Initializer()
			{
				HttpClientInitializer = credential,
				ApplicationName = "GoogleAnalyticsAPI",
			});

			var startDate = DateTime.Now.AddMonths(-1).AddDays(-2).ToString("yyyy-MM-dd");
			var endDate = DateTime.Now.AddDays(+1).ToString("yyyy-MM-dd");

			var request = service.Data.Ga.Get("ga:162383562", startDate, endDate, "ga:visits");
			request.Dimensions = "ga:date";
			var report = request.Execute();
			int sum = 0;
			foreach (var element in report.Rows)
				sum = sum + Int32.Parse(element[1]);
			return sum;
		}

		//--------------------------------------------------------------------------------

		[HttpGet, Route("myjobs")]
		[Authorize(Roles = "Client")]
		public ActionResult MyJobs(string currentFilter, string searchString, int? page)
		{
			int PageSize = 10;
			int pageIndex = 1;
			int pageNumber = (page ?? 1);
			ViewBag.CurrentFilter = searchString;
			if (searchString != null)
				page = 1;
			else
				searchString = currentFilter;
			pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
			var client = _clientServices.RetrieveClient(this.GetLoggedInUser().ID);
			var jobsVM = this._clientServices.GetJobsViewModelList(client.ID);
			if (!String.IsNullOrEmpty(searchString))
				jobsVM = jobsVM.Where(s => s.Title.ToLower().Contains(searchString.ToLower())
									 || s.Description.ToLower().Contains(searchString.ToLower())).ToList();
			return View(jobsVM.ToPagedList(pageNumber, PageSize));
		}

		[HttpGet, Route("job/create-new-job")]
		[Authorize(Roles = "Client")]
		public ActionResult CreateNewJob()
		{
			var client = _clientServices.RetrieveClient(this.GetLoggedInUser().ID);
			var model = mapNewJobViewModel(client.ID);
			return View(model);
		}

		[HttpPost, Route("job/create-new-job")]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Client")]
		public ActionResult CreateNewJob(NewJobViewModel model)
		{
			var client = this.GetLoggedInUser();
			if (ModelState.IsValid && client != null)
			{
				this._clientServices.CreateNewJob(client.ID, model);
				return RedirectToAction("myjobs");
			}
			return RedirectToAction("create-new-job");
		}

		[HttpGet, Route("job/update-job/{id:int}")]
		[Authorize(Roles = "Client")]
		public ActionResult UpdateJob(int id)
		{
			var client = _clientServices.RetrieveClient(this.GetLoggedInUser().ID);
			var model = this._clientServices.GetNewJobViewModel(client.ID, id);
			if (model == null)
				return RedirectToAction("myjobs");
			return View(model);
		}

		[HttpPost, Route("job/update-job")]
		[Authorize(Roles = "Client")]
		public ActionResult UpdateJob(NewJobViewModel model)
		{
			var client = this.GetLoggedInUser();
			if (ModelState.IsValid && client != null)
			{
				this._clientServices.UpdateJob(client.ID, model);
				return RedirectToAction("myjobs");
			}
			return RedirectToAction("update-job/" + model.ID);
		}

		[HttpGet]
		[Authorize(Roles = "Client")]
		public ActionResult Preview(NewJobViewModel model)
		{
			return PartialView("_JobPreview", model);
		}

		[HttpGet]
		public JsonResult GetPatientInfo(string id)
		{
			var client = _clientServices.RetrieveClient(this.GetLoggedInUser().ID);
			var patient = this._clientServices.RetrievePatient(client.ID, int.Parse(id));
			DateTime now = DateTime.Today;
			DateTime dob = patient.DoB ?? DateTime.Now;
			int age = now.Year - dob.Year;
			if (now < dob.AddYears(age))
				age--;
			var interests = this._presentationService.GetSubCategoryListByUser("Interest", patient.ID);
			string tag = "";
			foreach (var element in interests)
			{
				tag = element.Name + ", " + tag;
			}

			List<SelectListItem> results = new List<SelectListItem>();
			results.Add(new SelectListItem { Text = "full-address", Value = patient.Address.FullAddress });
			results.Add(new SelectListItem { Text = "Address_Suburb", Value = patient.Address.Suburb });
			results.Add(new SelectListItem { Text = "Address_State", Value = patient.Address.State });
			results.Add(new SelectListItem { Text = "Address_Postcode", Value = patient.Address.Postcode.ToString() });
			results.Add(new SelectListItem { Text = "Address_Latitude", Value = patient.Address.Latitude.ToString() });
			results.Add(new SelectListItem { Text = "Address_Longitude", Value = patient.Address.Longitude.ToString() });
			return Json(results, JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public JsonResult GetExistingSchedules(int jobID)
		{
			var client = this.GetLoggedInUser();
			var eventDetails = this._clientServices.GetExistingSchedules(client.ID, jobID);
			return Json(eventDetails.ToArray(), JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public JsonResult DeleteJobs(string clientID, string selectedJobsID)
		{
			var client = this.GetLoggedInUser();
			var jobIDs = selectedJobsID.Split(',').Select(int.Parse).ToList();
			var results = false;
			if (clientID != null && selectedJobsID != null)
				results = this._clientServices.DeleteJobs(client.ID, selectedJobsID);
			return Json(results, JsonRequestBehavior.AllowGet);
		}

		#endregion


		#region Helper

		private JobViewModel mapJobToJobViewModel(Job job)
		{
			JobViewModel model = new JobViewModel();
			model = _mapper.Map<Job, JobViewModel>(job);
			return model;
		}

		private NewJobViewModel mapNewJobViewModel(int clientID)
		{
			NewJobViewModel model = new NewJobViewModel();
			model.ClientID = clientID;
			model.GenderDropDownList = this._presentationService.GetSubCategorySelectList("Gender");
			model.PatientDropDownList = this._presentationService.GetPatientSelectList(clientID);
			return model;
		}

		#endregion

	}
}