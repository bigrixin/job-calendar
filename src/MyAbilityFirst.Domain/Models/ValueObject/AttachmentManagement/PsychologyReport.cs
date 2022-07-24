using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAbilityFirst.Domain.AttachmentManagement
{
	public class PsychologyReport : UserAttachment
	{

		#region Ctor

		protected PsychologyReport()
		{
			// required by EF
		}

		public PsychologyReport(int userID) : this(userID, null)
		{
		}

		public PsychologyReport(int userID, string url) : base(userID, url)
		{
		}

		#endregion

		#region Helpers

		public override AttachmentType GetAttachmentType() => AttachmentType.PsychologyReport;

		public override string GetFriendlyName() => "Psychology Report";

		#endregion

	}
}