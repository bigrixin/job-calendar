using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAbilityFirst.Domain.AttachmentManagement
{
	public class NdisPlanDocument : UserAttachment
	{

		#region Ctor

		protected NdisPlanDocument()
		{
			// required by EF
		}

		public NdisPlanDocument(int userID) : this(userID, null)
		{
		}

		public NdisPlanDocument(int userID, string url) : base(userID, url)
		{
		}

		#endregion

		#region Helpers

		public override AttachmentType GetAttachmentType() => AttachmentType.NdisPlanDocument;

		public override string GetFriendlyName() => "NDIS Plan Document";

		#endregion

	}
}