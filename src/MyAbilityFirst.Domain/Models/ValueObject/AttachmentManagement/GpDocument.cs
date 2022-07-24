using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAbilityFirst.Domain.AttachmentManagement
{
	public class GpDocument : UserAttachment
	{

		#region Ctor

		protected GpDocument()
		{
			// required by EF
		}

		public GpDocument(int userID) : this(userID, null)
		{
		}

		public GpDocument(int userID, string url) : base(userID, url)
		{
		}

		#endregion

		#region Helpers

		public override AttachmentType GetAttachmentType() => AttachmentType.GpDocument;

		public override string GetFriendlyName() => "GP Document";

		#endregion

	}
}