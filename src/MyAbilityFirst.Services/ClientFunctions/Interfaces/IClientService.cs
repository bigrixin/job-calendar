using MyAbilityFirst.Domain;
using MyAbilityFirst.Domain.ClientFunctions;
using MyAbilityFirst.Models;
using System.Collections.Generic;

namespace MyAbilityFirst.Services.ClientFunctions
{
	public interface IClientService
	{
		// Client Services
		Client RetrieveClient(int clientID);
		Client RetrieveClientByLoginID(string identityId);
		Client UpdateClient(Client updatedClient);

		// Patient Services
		Patient CreatePatient(int clientID, Patient patientData);
		Patient RetrievePatient(int clientID, int patientID);
		List<Patient> RetrieveAllPatients(int clientID);
		Patient UpdatePatient(int clientID, Patient patientData);

		// Category Services
		UserSubcategory CreateUserSubcategory(int ownerUserID, UserSubcategory userSubcategoryData);
		UserSubcategory RetrieveUserSubcategoryBySubcategoryID(int ownerUserID, int subcategoryID);
		List<UserSubcategory> RetrieveAllUserSubcategories(int ownerUserID);
		UserSubcategory UpdateUserSubcategory(int ownerUserID, UserSubcategory userSubcategoryData);
		List<UserSubcategory> ReplaceAllUserSubCategories(int ownerUserID, int[] postedSubCategoryIDs, List<UserSubcategory> customValueList);

		// Contact Services
		Contact CreateContact(int clientID, int patientID, Contact contactData);
		Contact RetrieveContact(int clientID, int patientID, int contactID);
		List<Contact> RetrieveAllContacts(int clientID, int patientID);
		Contact UpdateContact(int clientID, int patientID, Contact contactData);
		bool DeleteContact(int clientID, int patientID, int contactID);
		List<Contact> ReplaceAllContacts(int clientID, int patientID, List<Contact> contacts);

		Job PostNewJob(int ownerClientId, JobViewModel model);
		void EditJob(int ownerClientId, JobViewModel model);
		void DeleteJob(int ownerClientId, int jobId);

		// Booking Services
		Booking CreateNewBooking(int clientID, int careWorkerID, Schedule schedule, string message);
		void CancelBooking(int clientId, int bookingId);
		IEnumerable<NewBookingShortlistViewModel> GetShortlist(int ownerUserID);
		void UpdateBooking(int bookingOwnerID, UpdateBookingViewModel bookingDetails);
		void CompleteBooking(int clientID, int bookingID);
		UpdateBookingViewModel GetBookingViewModel(int bookingID, int clientID);
		IEnumerable<UpdateBookingViewModel> GetListOfBookingsViewModel(int clientID);
		void AddRating(int clientID, RatingViewModel ratingData);
		void UpdateRating(int clientID, double oldRating, RatingViewModel ratingData);
		RatingViewModel GetRatingVM(int clientID, int bookingID);


		// Job Services
		void CreateNewJob(int clientID, NewJobViewModel model);
		NewJobViewModel GetNewJobViewModel(int clientID, int jobID);
		List<JobsViewModel> GetJobsViewModelList(int clientID);
		void UpdateJob(int clientID, NewJobViewModel model);
		bool DeleteJobs(int clientID, string selectedJobsID);
		IList<CalendarEventsModel> GetExistingSchedules(int clientID,int jobID);
	}
}