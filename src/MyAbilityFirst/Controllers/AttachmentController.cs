using MyAbilityFirst.Domain.AttachmentManagement;
using MyAbilityFirst.Infrastructure;
using MyAbilityFirst.Services.AttachmentManagement;
using System.Web;
using System.Web.Mvc;

namespace MyAbilityFirst.Controllers
{
	[Authorize]
	[AuthorizeForAttachments]
	public class AttachmentController : Controller
	{

		#region Fields

		private readonly IReadEntities _entities;
		private readonly IAttachmentService _attachmentService;

		#endregion

		#region Ctor

		public AttachmentController(IReadEntities entities, IAttachmentService attachmentService)
		{
			this._entities = entities;
			this._attachmentService = attachmentService;
		}

		#endregion

		#region Actions

		public ActionResult Upload(AttachmentType itemId, int userID)
		{
			UserAttachment attached = null;

			HttpPostedFileBase file = Request.Files[0];
			if (file != null)
			{
				switch (itemId)
				{
					case AttachmentType.ProfilePhoto:
						attached = this._attachmentService.UploadAttachmentForPatient<ProfilePhoto>(userID, file);
						break;
					case AttachmentType.CarePlanDocument:
						attached = this._attachmentService.UploadAttachmentForPatient<CarePlanDocument>(userID, file);
						break;
					case AttachmentType.NdisPlanDocument:
						attached = this._attachmentService.UploadAttachmentForPatient<NdisPlanDocument>(userID, file);
						break;
					case AttachmentType.GpDocument:
						attached = this._attachmentService.UploadAttachmentForPatient<GpDocument>(userID, file);
						break;
					case AttachmentType.BirthCertificate:
						attached = this._attachmentService.UploadAttachmentForPatient<BirthCertificate>(userID, file);
						break;
					case AttachmentType.MedicareDocument:
						attached = this._attachmentService.UploadAttachmentForPatient<MedicareDocument>(userID, file);
						break;
					case AttachmentType.ProofOfAgeDocument:
						attached = this._attachmentService.UploadAttachmentForPatient<ProofOfAgeDocument>(userID, file);
						break;
					case AttachmentType.PsychologyReport:
						attached = this._attachmentService.UploadAttachmentForPatient<PsychologyReport>(userID, file);
						break;
					case AttachmentType.ReviewAssessmentReport:
						attached = this._attachmentService.UploadAttachmentForPatient<ReviewAssessmentReport>(userID, file);
						break;
				}
			}

			if (attached != null && !string.IsNullOrWhiteSpace(attached.URL))
				return Ok("File uploaded", attached.URL);

			return Error("Failed to upload");
		}

		public ActionResult Delete(AttachmentType itemId, int userID)
		{
			var ok = false;

			switch (itemId)
			{
				case AttachmentType.ProfilePhoto:
					ok = this._attachmentService.DeleteAttachmentForPatient<ProfilePhoto>(userID);
					break;
				case AttachmentType.CarePlanDocument:
					ok = this._attachmentService.DeleteAttachmentForPatient<CarePlanDocument>(userID);
					break;
				case AttachmentType.NdisPlanDocument:
					ok = this._attachmentService.DeleteAttachmentForPatient<NdisPlanDocument>(userID);
					break;
				case AttachmentType.GpDocument:
					ok = this._attachmentService.DeleteAttachmentForPatient<GpDocument>(userID);
					break;
				case AttachmentType.BirthCertificate:
					ok = this._attachmentService.DeleteAttachmentForPatient<BirthCertificate>(userID);
					break;
				case AttachmentType.MedicareDocument:
					ok = this._attachmentService.DeleteAttachmentForPatient<MedicareDocument>(userID);
					break;
				case AttachmentType.ProofOfAgeDocument:
					ok = this._attachmentService.DeleteAttachmentForPatient<ProofOfAgeDocument>(userID);
					break;
				case AttachmentType.PsychologyReport:
					ok = this._attachmentService.DeleteAttachmentForPatient<PsychologyReport>(userID);
					break;
				case AttachmentType.ReviewAssessmentReport:
					ok = this._attachmentService.DeleteAttachmentForPatient<ReviewAssessmentReport>(userID);
					break;
			}

			if (ok)
				return Ok("File deleted", null);

			return Error("Failed to delete");
		}

		private JsonResult Ok(string message, string url)
		{
			return
				Json(new
				{
					statusCode = 200,
					status = "OK",
					message = message,
					file = url,
				}, "text/html");
		}

		private JsonResult Error(string message)
		{
			return
				Json(new
				{
					statusCode = 500,
					status = "Error",
					message = message
				}, "text/html");
		}

		#endregion

	}
}