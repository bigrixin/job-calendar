using MyAbilityFirst.Domain;

namespace MyAbilityFirst.Infrastructure.Communication
{
	public interface IEmailService
	{
		void SendBookingRequestedEmail(Booking booking, string subject);
		void SendBookingCancelledEmail(Booking booking, string subject);
		void SendBookingAcceptedEmail(Booking booking, string subject);
		void SendBookingRejectedEmail(Booking booking, string subject);
		void SendBookingUpdatedByClientEmail(Booking booking, string subject);
		void SendBookingUpdatedByCareWorkerEmail(Booking booking, string subject);
		void SendBookingCompletedEmail(Booking booking, string subject);
		void SendBookingRatedEmail(Booking booking, string subject);
		void SendBookingRatingUpdatedEmail(Booking booking, string subject);
		void SendBookingReplacementRequestEmail(Booking booking, string subject);
		void SendBookingReplacementToClientEmail(Booking booking, string subject);
		void SendBookingReplacementToPreviousCarerEmail(Booking booking, string subject);
	}
}