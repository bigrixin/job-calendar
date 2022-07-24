using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAbilityFirst.Domain.AttachmentManagement
{
	public class ReviewAssessmentReport : UserAttachment
	{

		#region Ctor

		protected ReviewAssessmentReport()
		{
			// required by EF
		}

		public ReviewAssessmentReport(int userID) : this(userID, null)
		{
		}

		public ReviewAssessmentReport(int userID, string url) : base(userID, url)
		{
		}

		#endregion

		#region Helpers

		public override AttachmentType GetAttachmentType() => AttachmentType.ReviewAssessmentReport;

		public override string GetFriendlyName() => "Review & Assessment Report";

		#endregion

	}
}