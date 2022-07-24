using AutoMapper;
using MyAbilityFirst.Domain;
using MyAbilityFirst.Models;
using MyAbilityFirst.Services.ClientFunctions;
using MyAbilityFirst.Services.Common;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;

namespace MyAbilityFirst.Controllers
{
	[Authorize(Roles = "Client")]
	public class PatientController : Controller
	{

		#region Fields

		private readonly IClientService _clientServices;
		private readonly IPresentationService _categoryServices;
		private readonly IMapper _mapper;
		private readonly IUploadService _uploadServices;

		#endregion

		#region Ctor

		public PatientController(IClientService clientServices, IPresentationService categoryServices, IMapper mapper, IUploadService uploadServices)
		{
			this._clientServices = clientServices;
			this._categoryServices = categoryServices;
			this._mapper = mapper;
			this._uploadServices = uploadServices;
		}

		#endregion

		#region Actions

		[HttpGet, Route("patient/new")]
		public ActionResult NewProfile()
		{
			var client = this.GetLoggedInUser();
			PatientDetailsViewModel vm = mapPatientToVM(new Patient(client.ID));
			return View(vm);
		}

		[HttpPost, Route("patient/new")]
		[ValidateAntiForgeryToken]
		public ActionResult NewProfile(PatientDetailsViewModel vm)
		{
			if (ModelState.IsValid)
			{
				var client = this.GetLoggedInUser();
				Patient persistedPatient = _clientServices.CreatePatient(client.ID, mapVMToPatient(vm));
				vm.PatientID = persistedPatient.ID;
				_clientServices.ReplaceAllContacts(client.ID, persistedPatient.ID, vm.Contacts);

				persistCategories(vm);
			}

			return RedirectToAction("MyAccount", "Client");
		}

		[HttpGet, Route("patient/{patientID:int}")]
		public ActionResult EditProfile(int patientID)
		{
			var client = this.GetLoggedInUser();
			Patient currentPatient = _clientServices.RetrievePatient(client.ID, patientID);
			if (currentPatient != null)
			{
				PatientDetailsViewModel vm = mapPatientToVM(currentPatient);
				return View(vm);
			}


			return RedirectToAction("MyAccount", "Client");
		}

		[HttpPost, Route("patient/{patientID:int}")]
		[ValidateAntiForgeryToken]
		public ActionResult EditProfile(PatientDetailsViewModel vm)
		{
			var client = this.GetLoggedInUser();
			Patient currentPatient = _clientServices.RetrievePatient(client.ID, vm.PatientID);
			if (ModelState.IsValid && currentPatient != null)
			{
				Patient persistedPatient = _clientServices.UpdatePatient(client.ID, mapVMToPatient(vm));
				_clientServices.ReplaceAllContacts(client.ID, persistedPatient.ID, vm.Contacts);

				persistCategories(vm);
				return RedirectToAction("MyAccount/" + persistedPatient.ID.ToString());
			}
			return RedirectToAction("MyAccount", "Client");
		}

		[HttpGet, Route("patient/myaccount/{id:int}")]
		public ActionResult MyAccount(int id)
		{
			var client = this.GetLoggedInUser();
			Patient currentPatient = _clientServices.RetrievePatient(client.ID, id);
			if (currentPatient != null)
			{
				PatientDetailsViewModel vm = mapPatientToVM(currentPatient);
				return View(vm);
			}
			return RedirectToAction("MyAccount", "Client");
		}

		[HttpPost]
		public ActionResult NewContact(PatientDetailsViewModel vm)
		{
			Contact newContact = new Contact();
			newContact.PatientID = vm.PatientID;
			vm.Contacts = vm.Contacts ?? new List<Contact>();
			vm.RelationshipDropDownList = _categoryServices.GetSubCategorySelectList("Relationship");
			vm.Contacts.Add(newContact);
			return PartialView("_NewContact", vm);
		}

		[HttpGet, Route("patient/{patientID:int}/attachments/")]
		public ActionResult ManageAttachments(int patientID)
		{
			var client = this.GetLoggedInUser();
			Patient currentPatient = _clientServices.RetrievePatient(client.ID, patientID);
			if (currentPatient != null)
			{
				PatientAttachmentViewModel vm = _mapper.Map<Patient, PatientAttachmentViewModel>(currentPatient);
				ViewBag.PathUpload = Url.Action("Upload", "Attachment");
				ViewBag.PathDelete = Url.Action("Delete", "Attachment");

				return View(vm);
			}

			return RedirectToAction("MyAccount", "Client");
		}

		#endregion

		#region Helper

		private void persistCategories(PatientDetailsViewModel vm)
		{
			_clientServices.ReplaceAllUserSubCategories(vm.PatientID, vm.PostedSubCategoryIDs, new List<UserSubcategory>());
		}

		private PatientDetailsViewModel mapPatientToVM(Patient patient)
		{
			PatientDetailsViewModel vm = new PatientDetailsViewModel();
			vm = _mapper.Map<Patient, PatientDetailsViewModel>(patient);
			return vm;
		}

		private Patient mapVMToPatient(PatientDetailsViewModel vm)
		{
			var client = this.GetLoggedInUser();
			Patient patient = _clientServices.RetrievePatient(client.ID, vm.PatientID);
			vm.ClientID = client.ID;
			return _mapper.Map(vm, patient);
		}

		#endregion

	}
}