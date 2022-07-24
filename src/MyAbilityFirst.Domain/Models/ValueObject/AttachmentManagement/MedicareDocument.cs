using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAbilityFirst.Domain.AttachmentManagement
{
	public class MedicareDocument : UserAttachment
	{

		#region Ctor

		protected MedicareDocument()
		{
			// required by EF
		}

		public MedicareDocument(int userID) : this(userID, null)
		{
		}

		public MedicareDocument(int userID, string url) : base(userID, url)
		{
		}

		#endregion

		#region Helpers

		public override AttachmentType GetAttachmentType() => AttachmentType.MedicareDocument;

		public override string GetFriendlyName() => "Medicare Document";

		#endregion

	}
}