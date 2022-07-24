using MyAbilityFirst.Domain.AttachmentManagement;
using System.Collections.Generic;

namespace MyAbilityFirst.Models
{
	public class PatientAttachmentViewModel
	{
		public int PatientID { get; set; }
		public int ClientID { get; set; }

		// all attachments list
		public List<AttachmentType> AttachmentList { get; set; }

		// for tracking changes, contains previously uploaded attachment
		public List<UserAttachment> PreviousAttachmentList { get; set; }
	}
}