using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAbilityFirst.Domain.AttachmentManagement
{
	public abstract class UserAttachment
	{

		#region Properties

		public int ID { get; private set; }
		public int UserID { get; private set; }
		public string URL { get; protected set; }

		#endregion

		#region Ctor

		protected UserAttachment()
		{
			// required by EF
		}

		protected UserAttachment(int userID)
		{
			this.UserID = userID;
		}

		protected UserAttachment(int userID, string url)
		{
			this.UserID = userID;
			this.URL = url;
		}

		#endregion

		#region Helpers

		public void RemoveUrl() => this.URL = string.Empty;

		public void SetUrl(string url)
		{
			if (string.IsNullOrEmpty(url) || url.Equals(this.URL, StringComparison.OrdinalIgnoreCase))
				return;

			this.URL = url;
		}

		public abstract AttachmentType GetAttachmentType();

		public abstract string GetFriendlyName();

		#endregion

	}
}