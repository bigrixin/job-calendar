using MyAbilityFirst.Domain;
using MyAbilityFirst.Domain.AttachmentManagement;
using MyAbilityFirst.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MyAbilityFirst.Services.Common
{
	public class PresentationService : IPresentationService
	{

		#region Fields

		private readonly IReadEntities _entities;

		#endregion

		#region Ctor

		public PresentationService(IReadEntities entities)
		{
			_entities = entities;
		}

		#endregion

		#region PresentationService

		public List<Availability> GetPrefilledAvailabilityList(int careWorkerID, ICollection<Availability> selectedAvailabilities)
		{
			List<DayOfWeek> DayOfWeekList = GetDayOfWeekList();
			List<TimeOfDay> TimeOfDayList = GetTimeOfDayList();
			List<Availability> result = selectedAvailabilities.ToList();

			foreach (DayOfWeek dow in DayOfWeekList)
			{
				foreach (TimeOfDay tod in TimeOfDayList)
				{
					if (!result.Exists(
						av => av.CareWorkerID == careWorkerID
						&& av.DayOfWeek == dow
						&& av.TimeOfDay == tod
					))
					{
						Availability a = new Availability();
						a.CareWorkerID = careWorkerID;
						a.DayOfWeek = dow;
						a.TimeOfDay = tod;
						a.Selected = false;
						result.Add(a);
					}
				}
			}

			return result;
		}

		public string GetSubCategoryName(int subcategoryID)
		{
			if (subcategoryID <= 0)
				return null;

			var subcategory = this._entities.Single<Subcategory>(sc => sc.ID == subcategoryID);
			return subcategory != null ? subcategory.Name : null;
		}

		public List<DayOfWeek> GetDayOfWeekList()
		{
			return Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>().ToList();
		}

		public List<TimeOfDay> GetTimeOfDayList()
		{
			return Enum.GetValues(typeof(TimeOfDay)).Cast<TimeOfDay>().ToList();
		}

		public SelectList GetSubCategorySelectList(string categoryName)
		{
			return new SelectList(
					getSubCategory(getCategory(categoryName).ID),
					"ID",
					"Name");
		}

		public List<Subcategory> GetSubCategoryList(string categoryName)
		{
			return getSubCategory(getCategory(categoryName).ID);
		}

		public List<Subcategory> GetSubCategoryListByUser(string categoryName, int userID)
		{
			int categoryID = getCategory(categoryName).ID;
			IEnumerable<Subcategory> scs = getSubCategory(categoryID);
			IEnumerable<UserSubcategory> usc = this._entities.Get<UserSubcategory>(x => x.OwnerUserID == userID && x.Selected == true);
			return scs.Where(x => usc.Any(y => y.SubCategoryID == x.ID)).ToList();
		}

		public SelectList GetPatientSelectList(int id)
		{
			return new SelectList(
			getPatientList(id),
			"ID",
			"LastName");
		}

		public List<Subcategory> GetCrossReferenceSubcategoryList(int[] selectedSubcategoryIDs, List<Subcategory> referenceList)
		{
			selectedSubcategoryIDs = selectedSubcategoryIDs ?? new int[0];
			referenceList = referenceList ?? new List<Subcategory>();

			List<Subcategory> res = (
				from r in referenceList
				where selectedSubcategoryIDs.Contains(r.ID)
				select r
			).ToList();

			return res;
		}

		public Address GetUserAddress(int userID)
		{
			var user = _entities.Single<User>(u => u.ID == userID);
			if (user != null)
				return user.Address;

			return null;
		}

		public List<AttachmentType> GetAttachmentList()
		{
			IEnumerable<AttachmentType> res = Enum.GetValues(typeof(AttachmentType)).Cast<AttachmentType>();
			return res.ToList();
		}

		public SelectList GetOrganisationList()
		{
			return new SelectList(
			this._entities.Get<Organisation>().ToList(),
			"ID", "Name"
			);
		}

		public string GetOrganisationLogoURL(int organistionID)
		{
			return this._entities.Single<Organisation>(o => o.ID == organistionID).LogoURL;
		}

		public Organisation GetOrganisation(int organistionID)
		{
			return this._entities.Single<Organisation>(o => o.ID == organistionID);
		}

		public int GetPreviousBookingID(int bookingID)
		{
			var replacement = this._entities.Single<Replacement>(r => r.ReplacedBookingID == bookingID);
			if (replacement == null)
				return 0;
			var booking = this._entities.Get<Booking>(a => a.ID == replacement.BookingID).FirstOrDefault();
			if (booking == null)
				return 0;
			return booking.ID;
		}

		public string GetUserFirstLastName(int userID)
		{
			var user = _entities.Single<User>(u => u.ID == userID);
			if (user != null)
				return user.FirstName + " " + user.LastName;

			return null;
		}

		public string GetUserPictureUrl(int userID)
		{
			return "https://www.w3schools.com/bootstrap/img_avatar3.png";
		}

		public int GetMessagesCount(int roomID)
		{
			return _entities.Count<Message>(m => m.ChatRoomID == roomID);
		}

		public string GetGenderByID(int id)
		{
			return getSubCategory(getCategory("Gender").ID).Where(a => a.ID == id).FirstOrDefault().Name;
		}

		#endregion

		#region Job

		public List<bool> GetSelectedPatientsStatus(List<int> patients, int clientID)
		{
			var allPatients = this._entities.Single<Client>(c => c.ID == clientID).Patients;
			List<int> allPatientList = allPatients.Select(p => p.ID).ToList().OrderBy(s => s).ToList();
			patients = patients.OrderBy(s => s).ToList();
			List<bool> selectedPatients = new List<bool>();
			for (int i = 0; i < allPatientList.Count(); i++)
			{
				selectedPatients.Add(patients.Contains(allPatientList[i]));
			}
			return selectedPatients;
		}

		public List<bool> GetSelectedDayofWeek(List<Schedule> schedules)
		{
			List<bool> selectedDayofWeek = new List<bool>();
			for (int index = 0; index < 7; index++)
			{
				selectedDayofWeek.Add(false);
			}

			foreach (var element in schedules)
			{
				var dayOfWeek = element.GetDayOfWeek();
				selectedDayofWeek[dayOfWeek] = true;
			}
			return selectedDayofWeek;
		}

		public List<bool> GetSelectedPatients(List<Patient> selectedPatients, int ClientID)
		{
			var patients = getPatientList(ClientID);
			List<bool> selectedPatientsBool = new List<bool>();
			foreach (var element in patients)
				selectedPatientsBool.Add(selectedPatients.Exists(p => p.ID == element.ID));
			return selectedPatientsBool;
		}

		#endregion

		#region Helper

		private Category getCategory(string categoryName)
		{
			return this._entities.Get<Category>(
			cc => cc.Name.Equals(categoryName),
			null).First();
		}

		private List<Subcategory> getSubCategory(int categoryID)
		{
			return this._entities.Get<Subcategory>(
			 sc => sc.CategoryID == categoryID,
			 o => o.OrderBy(sc => sc.ID)).ToList();
		}

		private List<Patient> getPatientList(int id)
		{
			return this._entities.Get<Patient>(a => a.ClientID == id).ToList();
		}

		#endregion

	}
}
