using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace MyAbilityFirst.Domain.Test
{
	[TestClass]
	public class BookingTest
	{

		#region Fields

		private static int someClientId;
		private static int someCareWorkerId;
		private static Schedule someSchedule;

		#endregion

		#region Test events

		[ClassInitialize]
		public static void ClassInitialize(TestContext context)
		{
			someClientId = 1;
			someCareWorkerId = 2;
		//	someSchedule = new Schedule(DateTime.Now, DateTime.Now.Add(new TimeSpan(6, 0, 0)), ScheduleType.OneOff) { ID = 1 };
		}

		#endregion

		#region Test methods

		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void Should_Throw_Exception_When_Schedule_Start_Is_After_End()
		{
			// Arrange
//			var schedule = new Schedule(DateTime.Now, DateTime.Now.Add(new TimeSpan(-1, 0, 0)), ScheduleType.OneOff) { ID = 1 };

		//	var booking = new Booking(2, 1, schedule.ID);

			// Act
			// nothing to do

			// Assert
			// see ExpectedException attribute
		}

		[TestMethod]
		public void Can_Add_CaseNote_To_CaseNotesCollection()
		{
			// Arrange
			var booking = new Booking(someClientId, someCareWorkerId, someSchedule);
		//	var booking = new Booking(someCareWorkerId, 1, someSchedule.ID);

			// Act
			booking.AddCaseNote(someClientId, "This is a test note");

			// Assert
			Assert.IsTrue(booking.GetCaseNotes().Count == 1);

			var caseNote = booking.GetCaseNotes().First();
			Assert.IsTrue(caseNote.Note == "This is a test note");
		}

		[TestMethod]
		public void Can_Get_CaseNote_By_Id()
		{
			// Arrange
			var booking = new Booking(someClientId, someCareWorkerId, someSchedule);
			//var booking = new Booking(someCareWorkerId, 1, someSchedule.ID);
			booking.AddCaseNote(someClientId, "This is a test note");
			booking.AddCaseNote(someClientId, "This is another test note");

			var note1 = booking.GetCaseNotes().ElementAt(0);
			note1.ID = 1;

			var note2 = booking.GetCaseNotes().ElementAt(1);
			note2.ID = 2;

			// Act
			var noteToCheck = booking.GetCaseNote(2);

			// Assert
			Assert.IsTrue(noteToCheck.Note == "This is another test note");

		}

		[TestMethod]
		public void Can_Update_CaseNote()
		{
			// Arrange
			var booking = new Booking(someClientId, someCareWorkerId, someSchedule);
			//var booking = new Booking(someCareWorkerId, 1, someSchedule.ID);
			booking.AddCaseNote(someClientId, "This is a test note");

			var note = booking.GetCaseNotes().ElementAt(0);
			note.ID = 1;

			// Act
			var anotherNote = new CaseNote(1, someClientId, "This is an updated note");
			anotherNote.ID = 1;

			booking.UpdateCaseNote(anotherNote);

			// Assert
			var updatedNote = booking.GetCaseNote(1);
			Assert.IsTrue(anotherNote.Note == updatedNote.Note);
		}

		[TestMethod]
		public void Can_Delete_CaseNote()
		{
			// Arrange
			var booking = new Booking(someClientId, someCareWorkerId, someSchedule);
			booking.AddCaseNote(someClientId, "This is a test note");
			booking.AddCaseNote(someClientId, "This is another test note");

			var note1 = booking.GetCaseNotes().ElementAt(0);
			note1.ID = 1;

			var note2 = booking.GetCaseNotes().ElementAt(1);
			note2.ID = 2;

			// Act
			var noteToDelete = booking.DeleteCaseNote(1, someClientId);

			// Assert
			var noteToAssert = booking.GetCaseNotes().First();
			Assert.IsTrue(noteToAssert.Note == note2.Note);
		}

		[TestMethod]
		public void Can_Only_Delete_Own_CaseNote()
		{
			// Arrange
			var booking = new Booking(someClientId, someCareWorkerId, someSchedule);
		//	var booking = new Booking(someCareWorkerId, 1, someSchedule.ID);
			booking.AddCaseNote(someClientId, "This is a client note");
			booking.AddCaseNote(someCareWorkerId, "This is a carer note");

			var note1 = booking.GetCaseNotes().ElementAt(0);
			note1.ID = 1;

			var note2 = booking.GetCaseNotes().ElementAt(1);
			note2.ID = 2;

			var timestampBeforeDelete = booking.UpdatedAt;

			// Act
			var noteToDelete = booking.DeleteCaseNote(2, someClientId);

			// Assert: no note should be deleted
			Assert.IsTrue(booking.GetCaseNotes().Count == 2);

			Assert.IsTrue(booking.UpdatedAt == timestampBeforeDelete);
		}

		[TestMethod]
		public void Can_Get_All_CaseNotes()
		{
			// Arrange
			var booking = new Booking(someClientId, someCareWorkerId, someSchedule);
		//	var booking = new Booking(someCareWorkerId, 1, someSchedule.ID);
			booking.AddCaseNote(someClientId, "This is a client note #1");
			booking.AddCaseNote(someClientId, "This is a client note #2");
			booking.AddCaseNote(someCareWorkerId, "This is a carer note #1");
			booking.AddCaseNote(someClientId, "This is a client note #3");
			booking.AddCaseNote(someCareWorkerId, "This is a carer note #2");

			// Act
			var count = booking.GetCaseNotes().Count;

			// Assert
			Assert.IsTrue(count == 5);
		}

		[TestMethod]
		public void Can_Get_CaseNotes_By_UserId()
		{
			// Arrange
			var booking = new Booking(someClientId, someCareWorkerId, someSchedule);
			booking.AddCaseNote(someClientId, "This is a client note #1");
			booking.AddCaseNote(someClientId, "This is a client note #2");
			booking.AddCaseNote(someCareWorkerId, "This is a carer note #1");
			booking.AddCaseNote(someClientId, "This is a client note #3");
			booking.AddCaseNote(someCareWorkerId, "This is a carer note #2");

			// Act
			var count = booking.GetCaseNotesByUserId(someClientId).Count;

			// Assert
			Assert.IsTrue(count == 3);
		}
		/*
				[TestMethod]
				public void Can_Update_Schedule()
				{
					// Arrange
					var schedule = new Schedule(DateTime.Now, DateTime.Now.AddHours(1));
					var booking = new Booking(someClientId, someCareWorkerId, schedule);

					// Act
					var newSchedule = schedule.NewEnd(schedule.Start.AddHours(3));
					booking.UpdateSchedule(newSchedule);

					// Assert
					Assert.IsTrue((booking.Schedule.Duration.End - booking.Schedule.Duration.Start).TotalHours == 3);
				}
				*/
		[TestMethod]
		public void Can_Update_JobID()
		{
			// Arrange
			var booking = new Booking(someClientId, someCareWorkerId, someSchedule);

			// Act
			var someJobId = 1;
			booking.UpdateJobID(someJobId);

			// Assert
			Assert.IsTrue(booking.JobID == someJobId);
		}

		[TestMethod]
		public void Can_Be_Cancelled()
		{
			// Arrange
			var booking = new Booking(someClientId, someCareWorkerId, someSchedule);

			// Act
			booking.Cancel();

			// Assert
			Assert.IsTrue(booking.Status == BookingStatus.Cancelled);
		}

		[TestMethod]
		public void Can_Only_Be_Cancelled_If_Not_Completed()
		{
			// Arrange
			var booking = new Booking(someClientId, someCareWorkerId, someSchedule);

			// Act
			booking.Complete();
			booking.Cancel();

			// Assert
			Assert.IsTrue(booking.Status == BookingStatus.Cancelled);
		}

		[TestMethod]
		public void Can_Be_Accepted()
		{
			// Arrange
			var booking = new Booking(someClientId, someCareWorkerId, someSchedule);

			// Act
			booking.Accept();

			// Assert
			Assert.IsTrue(booking.Status == BookingStatus.Accepted);
		}

		[TestMethod]
		public void Can_Only_Be_Accepted_If_Not_Cancelled()
		{
			// Arrange
			var booking = new Booking(someClientId, someCareWorkerId, someSchedule);

			// Act
			booking.Cancel();
			booking.Accept();

			// Assert
			Assert.IsTrue(booking.Status == BookingStatus.Cancelled);
		}
		/*
		[TestMethod]
		public void Can_Only_Update_Schedule_If_Not_Cancelled()
		{
			// Arrange
			var schedule = new Schedule(DateTime.Now, DateTime.Now.AddHours(1));
			var booking = new Booking(someClientId, someCareWorkerId, schedule);
			booking.Cancel();

			// Act
			var newSchedule = schedule.NewEnd(schedule.Start.AddHours(3));
			booking.UpdateSchedule(newSchedule);

			// Assert
			Assert.IsTrue((booking.Schedule.Duration.End - booking.Schedule.Duration.Start).TotalHours == 1);
		}
	*/
		#endregion

	}
}