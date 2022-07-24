using System;

namespace MyAbilityFirst.Domain
{
	public class Replacement
	{

		#region Properties

		public int ID { get; set; }

		public int BookingID { get; private set; }
		public int CareWorkerID { get; set; }
		public string CommentByCareWorker { get; set; }

		public int? ReplacedCareWorkerID { get; set; }
		public int? ReplacedBookingID { get; set; }
		public int? CoordinatorID { get; set; }
		public string CommentByCoordinator { get; set; }

		public DateTime? CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }

		#endregion

		#region Ctor

		protected Replacement()
		{
			// required by EF

		}

		public Replacement(int bookingID, int careWorkerID, string commentByCareWorker)
		{
			this.BookingID = bookingID;
			this.CareWorkerID = careWorkerID;
			this.CommentByCareWorker = commentByCareWorker;
		}

		public Replacement(int bookingID, int careWorkerID, string commentByCareWorker, int? coordinatorID, int? replacedCareWorkerID, string commentByCoordinator)
		{
			this.BookingID = bookingID;
			this.CareWorkerID = careWorkerID;
			this.CommentByCareWorker = commentByCareWorker;
			this.CoordinatorID = coordinatorID;
			this.ReplacedCareWorkerID = replacedCareWorkerID;
			this.CommentByCoordinator = commentByCoordinator;
		}

		#endregion

	}
}
