using MyAbilityFirst.Domain.AttachmentManagement;
using System.Collections.Generic;
using System.Web;

namespace MyAbilityFirst.Services.AttachmentManagement
{
	public interface IAttachmentService
	{
		// patient attachments
		T UploadAttachmentForPatient<T>(int patientID, HttpPostedFileBase files) where T : UserAttachment;
		bool DeleteAttachmentForPatient<T>(int patientID) where T : UserAttachment;
		T GetAttachmentForPatient<T>(int patientID) where T : UserAttachment;

		// user attachments
		List<UserAttachment> GetAttachmentsForUser(int userID);
	}
}