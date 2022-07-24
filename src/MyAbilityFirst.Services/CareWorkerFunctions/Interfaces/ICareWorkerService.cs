using MyAbilityFirst.Domain;
using MyAbilityFirst.Domain.ClientFunctions;
using MyAbilityFirst.Models;
using System.Collections.Generic;

namespace MyAbilityFirst.Services.CareWorkerFunctions
{
	public interface ICareWorkerService
	{

		// CareWorker Services
		CareWorker RetrieveCareWorker(int careWorkerID);
		CareWorker RetrieveCareWorkerByLoginID(string identityId);
		CareWorker UpdateCareWorker(CareWorker updatedCareWorker);

		// Category Services
		UserSubcategory CreateUserSubcategory(int ownerUserID, UserSubcategory userSubcategoryData);
		UserSubcategory RetrieveUserSubcategoryBySubcategoryID(int ownerUserID, int subcategoryID);
		List<UserSubcategory> RetrieveAllUserSubcategories(int ownerUserID);
		UserSubcategory UpdateUserSubcategory(int ownerUserID, UserSubcategory userSubcategoryData);
		List<UserSubcategory> ReplaceAllUserSubCategories(int ownerUserID, int[] postedSubCategoryIDs, List<UserSubcategory> customValueList);

		// Employment History Services
		EmploymentHistory CreateEmploymentHistory(int careWorkerID, EmploymentHistory employmentHistoryData);
		EmploymentHistory RetrieveEmploymentHistory(int careWorkerID, int employmentHistoryID);
		List<EmploymentHistory> RetrieveAllEmploymentHistories(int careWorkerID);
		EmploymentHistory UpdateEmploymentHistory(int careWorkerID, EmploymentHistory employmentHistoryData);
		bool DeleteEmploymentHistory(int careWorkerID, int employmentHistoryID);
		List<EmploymentHistory> ReplaceAllEmploymentHistories(int careWorkerID, IEnumerable<EmploymentHistory> employmentHistories);

		// Employment Education Services
		EmploymentFormalEducation CreateEmploymentFormalEducation(int careWorkerID, EmploymentFormalEducation employmentFormalEducationData);
		EmploymentFormalEducation RetrieveEmploymentFormalEducation(int careWorkerID, int employmentFormalEducationID);
		List<EmploymentFormalEducation> RetrieveAllEmploymentFormalEducations(int careWorkerID);
		EmploymentFormalEducation UpdateEmploymentFormalEducation(int careWorkerID, EmploymentFormalEducation employmentFormalEducationData);
		bool DeleteEmploymentFormalEducation(int careWorkerID, int employmentFormalEducationID);
		List<EmploymentFormalEducation> ReplaceAllEmploymentFormalEducations(int careWorkerID, IEnumerable<EmploymentFormalEducation> employmentFormalEducations);

		// Employment Reference Services
		EmploymentReference CreateEmploymentReference(int careWorkerID, EmploymentReference employmentReferenceData);
		EmploymentReference RetrieveEmploymentReference(int careWorkerID, int employmentReferenceID);
		List<EmploymentReference> RetrieveAllEmploymentReferences(int careWorkerID);
		EmploymentReference UpdateEmploymentReference(int careWorkerID, EmploymentReference employmentReferenceData);
		bool DeleteEmploymentReference(int careWorkerID, int employmentReferenceID);
		List<EmploymentReference> ReplaceAllEmploymentReferences(int careWorkerID, IEnumerable<EmploymentReference> employmentReferences);

		// Employment Achievements Services
		EmploymentAchievement CreateEmploymentAchievement(int careWorkerID, EmploymentAchievement employmentAchievementData);
		EmploymentAchievement RetrieveEmploymentAchievement(int careWorkerID, int employmentAchievementID);
		List<EmploymentAchievement> RetrieveAllEmploymentAchievements(int careWorkerID);
		EmploymentAchievement UpdateEmploymentAchievement(int careWorkerID, EmploymentAchievement employmentAchievementData);
		bool DeleteEmploymentAchievement(int careWorkerID, int employmentAchievementID);
		List<EmploymentAchievement> ReplaceAllEmploymentAchievements(int careWorkerID, IEnumerable<EmploymentAchievement> employmentAchievements);

		// Availability Services
		Availability CreateAvailability(int careWorkerID, Availability availabilityData);
		Availability RetrieveAvailability(int careWorkerID, int availabilityID);
		List<Availability> RetrieveAllAvailabilites(int careWorkerID);
		Availability UpdateAvailability(int careWorkerID, Availability availabilityData);
		bool DeleteAvailability(int careWorkerID, int availabilityID);
		List<Availability> ReplaceAllAvailabilities(int careWorkerID, IEnumerable<Availability> availabilities);

		// Booking
		UpdateBookingViewModel GetBookingViewModel(int ownerUserID, int bookingID);
		IEnumerable<UpdateBookingViewModel> GetListOfBookingsViewModel(int careWorkerID);

		void UpdateBooking(int bookingOwnerID, UpdateBookingViewModel bookingDetails);
		void AcceptBooking(int careWorkerID, int bookingID);
		void RejectBooking(int careWorkerID, int bookingID);

		void AddOverallRating(int careWorkerID, Rating rating);
		void UpdateOverallRating(int careWorkerID, double oldRating, Rating rating);

		ReplacementViewModel GetReplacementVM(int carerID, int bookingID);
		void CreateReplacement(ReplacementViewModel vm);
		UpdateBookingViewModel GetPreviousBookingViewModel(int bookingID);
	}
}
