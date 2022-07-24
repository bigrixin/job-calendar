using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyAbilityFirst.Domain.Test
{
	[TestClass]
	public class ClientTest
	{

		#region Fields

		private static string someLoginIdentityId;
		private static int someClientId;
		private static int someCareWorkerId;
		private static int someJobId;
	//	private static Schedule scheduleA;
	//	private static Schedule scheduleB;
		private static Booking bookingA;
		private static Booking bookingB;

		#endregion

		#region Test events

		[ClassInitialize]
		public static void ClassInitialize(TestContext context)
		{
			someLoginIdentityId = "6f23b791-0151-40ea-8d26-b9145528190a";
			someClientId = 1;
			someCareWorkerId = 2;
			someJobId = 1;
		//	scheduleA = new Schedule(DateTime.Now, DateTime.Now.AddHours(1));
		//	scheduleB = new Schedule(DateTime.Now, DateTime.Now.AddHours(6));
		//	scheduleA = new Schedule(DateTime.Now, DateTime.Now.Add(new TimeSpan(1, 0, 0)), ScheduleType.OneOff, DateTime.Now.Add(new TimeSpan(1, 0, 0)));
		//	scheduleB = new Schedule(DateTime.Now, DateTime.Now.Add(new TimeSpan(6, 0, 0)), ScheduleType.OneOff);
	//		bookingA = new Booking(someClientId, someCareWorkerId, scheduleA) { ID = 1 };
	//		bookingB = new Booking(someClientId, someCareWorkerId, scheduleB) { ID = 2 };
		}

		#endregion

		#region Test methods

		[TestMethod]
		public void Can_Add_New_Booking()
		{
			// Arrange
			var client = new Client(someLoginIdentityId);
			client.CreateNewBooking(bookingA);
			client.CreateNewBooking(bookingB);

			// Act
			var bookings = client.GetBookings();

			// Assert
			Assert.IsTrue(bookings.Count == 2);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void Add_New_Booking_With_Already_Existing_ID_Should_Throw_Exception()
		{
			// Arrange
			var client = new Client(someLoginIdentityId);
			client.CreateNewBooking(bookingA);
			
			// Act
			client.CreateNewBooking(bookingA);

			// Assert
			// exception expected
		}

		[TestMethod]
		public void Can_Update_A_Booking()
		{
			// Arrange
			var client = new Client(someLoginIdentityId);
			client.CreateNewBooking(bookingA);
			client.CreateNewBooking(bookingB);

			// Act
		//	var newBooking = new Booking(someClientId, someCareWorkerId, someJobId, scheduleA) { ID = 1 };
		//	client.UpdateBooking(newBooking);

			// Assert
			var bookingToAssert = client.GetBooking(1);
			Assert.IsTrue(bookingToAssert.JobID.HasValue && bookingToAssert.JobID == someJobId);
		}

		[TestMethod]
		public void Can_Get_A_Booking_By_ID()
		{
			// Arrange
			var client = new Client(someLoginIdentityId);
			client.CreateNewBooking(bookingA);
			client.CreateNewBooking(bookingB);

			// Act
			var bookingToAssert = client.GetBooking(2);

			// Assert
			Assert.IsTrue(bookingToAssert.ID == 2);
		}

		[TestMethod]
		public void Can_Cancel_A_Booking()
		{
			// Arrange
			var client = new Client(someLoginIdentityId);
			client.CreateNewBooking(bookingA);
			client.CreateNewBooking(bookingB);

			// Act
			var bookingToCancel = client.GetBooking(1);
			bookingToCancel.Cancel();

			// Assert
			Assert.IsTrue(bookingToCancel.Status == BookingStatus.Cancelled);
		}

		#endregion

	}
}