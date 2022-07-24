using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAbilityFirst.Domain.AttachmentManagement
{
	public class ProfilePhoto : UserAttachment
	{

		#region Ctor

		protected ProfilePhoto()
		{
			// required by EF
		}

		public ProfilePhoto(int userID) : this(userID, null)
		{
		}

		public ProfilePhoto(int userID, string url) : base(userID, url)
		{
		}

		#endregion

		#region Helpers

		public override AttachmentType GetAttachmentType() => AttachmentType.ProfilePhoto;

		public override string GetFriendlyName() => "Profile Photo";

		#endregion

	}
}