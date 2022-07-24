using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAbilityFirst.Domain.AttachmentManagement
{
	public class ProofOfAgeDocument : UserAttachment
	{

		#region Ctor

		protected ProofOfAgeDocument()
		{
			// required by EF
		}

		public ProofOfAgeDocument(int userID) : this(userID, null)
		{
		}

		public ProofOfAgeDocument(int userID, string url) : base(userID, url)
		{
		}

		#endregion

		#region Helpers

		public override AttachmentType GetAttachmentType() => AttachmentType.ProofOfAgeDocument;

		public override string GetFriendlyName() => "Proof of Age";

		#endregion

	}
}