using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAbilityFirst.Domain.AttachmentManagement
{
	public class CarePlanDocument : UserAttachment
	{
		
		#region Ctor

		CarePlanDocument()
		{
			// required by EF
		}

		public CarePlanDocument(int userID) : this(userID, null)
		{
		}

		public CarePlanDocument(int userID, string url) : base(userID, url)
		{
		}

		#endregion

		#region Helpers

		public override AttachmentType GetAttachmentType() => AttachmentType.CarePlanDocument;

		public override string GetFriendlyName() => "Care Plan Document";

		#endregion

	}
}