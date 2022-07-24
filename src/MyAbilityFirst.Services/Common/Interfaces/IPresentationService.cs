using MyAbilityFirst.Domain;
using MyAbilityFirst.Domain.AttachmentManagement;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MyAbilityFirst.Services.Common
{
	public interface IPresentationService
	{
		List<Availability> GetPrefilledAvailabilityList(int careWorkerID, ICollection<Availability> selectedAvailabilities);
		string GetSubCategoryName(int subcategoryID);
		List<DayOfWeek> GetDayOfWeekList();
		List<TimeOfDay> GetTimeOfDayList();
		SelectList GetSubCategorySelectList(string categoryName);
		List<Subcategory> GetSubCategoryList(string categoryName);
		List<Subcategory> GetSubCategoryListByUser(string categoryName, int userID);
		SelectList GetPatientSelectList(int id);
		List<Subcategory> GetCrossReferenceSubcategoryList(int[] selectedSubcategoryIDs, List<Subcategory> referenceList);
		Address GetUserAddress(int userID);
		string GetUserFirstLastName(int userID);
		List<AttachmentType> GetAttachmentList();
		SelectList GetOrganisationList();
		string GetOrganisationLogoURL(int organistionID);
		Organisation GetOrganisation(int organistionID);
		string GetUserPictureUrl(int userID);
		int GetMessagesCount(int roomID);
		int GetPreviousBookingID(int bookingID);
		string GetGenderByID(int id);

		List<bool> GetSelectedPatientsStatus(List<int> patients, int clientID);
		List<bool> GetSelectedDayofWeek(List<Schedule> schedules);
		List<bool> GetSelectedPatients(List<Patient> selectedPatients, int ClientID);
	}
}
