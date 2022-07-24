using AutoMapper;
using MyAbilityFirst.Domain;
using MyAbilityFirst.Domain.ClientFunctions;
using MyAbilityFirst.Infrastructure;
using MyAbilityFirst.Infrastructure.Data;
using MyAbilityFirst.Models;
using MyAbilityFirst.Services.Common;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Globalization;
using System.Linq;
using MyAbilityFirst.Infrastructure.Communication;
using MyAbilityFirst.Infrastructure.Auth;
using System.Drawing;

namespace MyAbilityFirst.Services.ClientFunctions
{
	public class ClientService : UserService, IClientService
	{

		#region Fields

		private readonly IWriteEntities _entities;
		private readonly IMapper _mapper;
		private readonly BookingData _bookingData;
		private readonly IEmailService _emailServices;

		private readonly IPresentationService _presentationService;
		#endregion

		#region Ctor

		public ClientService(IWriteEntities entities, IMapper mapper, IBroadcastService hub, ILoginService loginServices, BookingData bookingData, IEmailService emailServices,  IPresentationService presentationService) : base(entities, mapper, hub, loginServices)
		{
			this._entities = entities;
			this._mapper = mapper;
			this._bookingData = bookingData;
			this._emailServices = emailServices;
			
			this._presentationService = presentationService;
		}

		#endregion

		#region IClientService

		public Client RetrieveClient(int clientID)
		{
			return this._entities.Single<Client>(c => c.ID == clientID);
		}

		public Client RetrieveClientByLoginID(string identityId)
		{
			return this._entities.Single<Client>(c => c.LoginIdentityId == identityId);
		}

		public Client UpdateClient(Client clientData)
		{
			clientData.Status = UserStatus.Active;
			clientData.UpdatedAt = DateTime.Now;
			this._entities.Update(clientData);
			this._entities.Save();
			return clientData;
		}

		public Patient CreatePatient(int clientID, Patient patientData)
		{
			Client parentClient = RetrieveClient(clientID);
			if (parentClient != null)
			{
				Patient newPatient = parentClient.AddNewPatient(patientData);
				this._entities.Update(parentClient);
				this._entities.Save();
				return newPatient;
			}
			return null;
		}

		public Patient RetrievePatient(int clientID, int patientID)
		{
			Client parentClient = RetrieveClient(clientID);
			if (parentClient != null)
			{
				return parentClient.GetExistingPatient(patientID);
			}
			return null;
		}

		public List<Patient> RetrieveAllPatients(int clientID)
		{
			Client parentClient = RetrieveClient(clientID);
			if (parentClient != null)
			{
				return parentClient.GetAllExistingPatients();
			}
			return null;
		}

		public Patient UpdatePatient(int clientID, Patient patientData)
		{
			Client parentClient = RetrieveClient(clientID);
			if (parentClient != null)
			{
				parentClient.UpdateExistingPatient(patientData);
				this._entities.Update(parentClient);
				this._entities.Save();
				return patientData;
			}
			return null;

		}

		public Contact CreateContact(int clientID, int patientID, Contact contactData)
		{
			Client parentClient = RetrieveClient(clientID);
			if (parentClient != null)
			{
				Patient parentPatient = parentClient.GetExistingPatient(patientID);
				if (parentPatient != null)
				{
					Contact newContact = parentPatient.AddNewContact(contactData);
					parentClient.UpdateExistingPatient(parentPatient);
					this._entities.Update(parentClient);
					this._entities.Save();
					return newContact;
				}
				return null;
			}
			return null;
		}

		public Contact RetrieveContact(int clientID, int patientID, int contactID)
		{
			Client parentClient = RetrieveClient(clientID);
			if (parentClient != null)
			{
				Patient parentPatient = parentClient.GetExistingPatient(patientID);
				if (parentPatient != null)
				{
					Contact contact = parentPatient.GetExistingContact(contactID);
					return contact;
				}
				return null;
			}
			return null;
		}

		public List<Contact> RetrieveAllContacts(int clientID, int patientID)
		{
			Client parentClient = RetrieveClient(clientID);
			if (parentClient != null)
			{
				Patient parentPatient = parentClient.GetExistingPatient(patientID);
				if (parentPatient != null)
				{
					var contacts = parentPatient.GetAllExistingContacts();
					return contacts;
				}
				return null;
			}
			return null;
		}

		public Contact UpdateContact(int clientID, int patientID, Contact contactData)
		{
			Client parentClient = RetrieveClient(clientID);
			if (parentClient != null)
			{
				Patient parentPatient = parentClient.GetExistingPatient(patientID);
				if (parentPatient != null)
				{
					Contact existingContact = RetrieveContact(clientID, patientID, contactData.ID);
					_mapper.Map(contactData, existingContact);
					parentPatient.UpdateExistingContact(existingContact);
					parentClient.UpdateExistingPatient(parentPatient);
					this._entities.Update(existingContact);
					this._entities.Save();
					return existingContact;
				}
				return null;
			}
			return null;
		}

		public bool DeleteContact(int clientID, int patientID, int contactID)
		{
			Client parentClient = RetrieveClient(clientID);
			if (parentClient != null)
			{
				Patient parentPatient = parentClient.GetExistingPatient(patientID);
				if (parentPatient != null)
				{
					Contact contact = parentPatient.RemoveContact(contactID);
					this._entities.Delete(contact);
					this._entities.Save();
					return true;
				}
				return false;
			}
			return false;
		}

		public List<Contact> ReplaceAllContacts(int clientID, int patientID, List<Contact> contacts)
		{
			List<Contact> existingContacts = RetrieveAllContacts(clientID, patientID) ?? new List<Contact>();
			contacts = contacts ?? new List<Contact>();
			IEnumerable<Contact> actionablecontacts = contacts.Except(existingContacts, new ContactComparer());
			// CREATE new Contact if old dataset doesn't have new contacts
			foreach (Contact contact in actionablecontacts)
			{
				contact.PatientID = patientID;
				CreateContact(clientID, contact.PatientID, contact);
			}
			// UPDATE Contact if already exists
			actionablecontacts = contacts.Intersect(existingContacts, new ContactComparer());
			foreach (Contact contact in actionablecontacts)
			{
				UpdateContact(clientID, contact.PatientID, contact);
			}
			// DELETE old Contact if new dataset doesn't have old contacts
			actionablecontacts = existingContacts.Except(contacts, new ContactComparer());
			foreach (Contact contact in actionablecontacts)
			{
				DeleteContact(clientID, contact.PatientID, contact.ID);
			}
			return contacts;
		}

		public Job PostNewJob(int ownerClientId, JobViewModel model)
		{
			if (ownerClientId < 1)
				throw new ArgumentException("ownerClientId must be an ID greater than 1.");

			var client = this._entities.Single<Client>(c => c.ID == ownerClientId);
			if (client == null)
				throw new ArgumentNullException("client");

			var now = DateTime.Now;
			var clientId = client.ID;

			var job = new Job(clientId);
			job = _mapper.Map<JobViewModel, Job>(model);
			job.CreatedAt = now;
			job.UpdatedAt = now;
			job.JobStatus = JobStatus.New;

			client.Jobs.Add(job);

			this._entities.Update(client);
			this._entities.Save();

			return job;
		}

		public void EditJob(int ownerClientId, JobViewModel model)
		{
			if (ownerClientId < 1)
				return;

			var client = this._entities.Single<Client>(c => c.ID == ownerClientId);
			if (client == null)
				throw new ArgumentNullException("client");

			var now = DateTime.Now;
			var clientId = client.ID;

			Job job = client.Jobs.SingleOrDefault<Job>(j => j.ID == model.Id);

			if (job != null)
			{
				job = _mapper.Map<JobViewModel, Job>(model, job);
				job.UpdatedAt = now;
				job.JobStatus = JobStatus.Updated;

				this._entities.Update(client);
				this._entities.Save();
			}
		}

		public void DeleteJob(int ownerClientId, int jobId)
		{
			if (ownerClientId < 1)
				return;

			var client = this._entities.Single<Client>(c => c.ID == ownerClientId);
			if (client == null)
				throw new ArgumentNullException("client");

			Job job = client.Jobs.SingleOrDefault(j => j.ID == jobId);
			if (job != null)
			{
				client.Jobs.Remove(job);
				this._entities.Delete(job);
				this._entities.Update(client);
				this._entities.Save();
			}
		}

		public Booking CreateNewBooking(int clientID, int careWorkerID, Schedule schedule, string message)
		{
			if (clientID < 1)
				throw new ArgumentOutOfRangeException("Value must be an int greater than 1", "clientID");

			if (careWorkerID < 1)
				throw new ArgumentOutOfRangeException("Value must be an int greater than 1", "careWorkerID");

			var client = this._entities.Single<Client>(c => c.ID == clientID);
			if (client == null)
				throw new ArgumentNullException("client");

			var carer = this._entities.Single<CareWorker>(cw => cw.ID == careWorkerID);
			if (carer == null)
				throw new ArgumentNullException("carer");

			var booking = new Booking(client.ID, carer.ID, schedule, message);

			// update case note
			var note = $"{client.FirstName} has created a request for help.";
			booking.AddCaseNote(client.ID, note);

			client.CreateNewBooking(booking);

			this._entities.Update(client);
			this._entities.Save();

			// send alert email to careworker
			this._emailServices.SendBookingRequestedEmail(booking, note);

			return booking;
		}

		public void CancelBooking(int clientID, int bookingID)
		{
			if (clientID < 1)
				throw new ArgumentOutOfRangeException("Value must be an int greater than 1", "clientID");

			if (bookingID < 1)
				throw new ArgumentOutOfRangeException("Value must be an int greater than 1", "bookingID");

			var client = this._entities.Single<Client>(c => c.ID == clientID);
			if (client == null)
				throw new ArgumentNullException("client");

			// get the booking to cancel or die
			var booking = client.GetBooking(bookingID);
			if (booking == null)
				throw new ArgumentNullException("booking");

			// cancel booking
			if (booking.Cancel())
			{
				// update case note
				var note = $"{client.FirstName} has canceled his help request.";
				booking.AddCaseNote(client.ID, note);

				this._entities.Update(client);
				this._entities.Save();

				// send alert email to client
				this._emailServices.SendBookingCancelledEmail(booking, note);
			}
		}

		public void UpdateBooking(int clientID, UpdateBookingViewModel bookingDetails)
		{
			var client = this._entities.Single<Client>(u => u.ID == clientID);
			if (client == null)
				throw new ArgumentNullException("bookingOwnerID not found as user");

			Booking booking = client.GetBooking(bookingDetails.BookingID);
			if (booking == null)
				throw new ArgumentNullException("booking");

			// update message
			booking.Message = bookingDetails.Message;

			// update schedule
			//			booking.UpdateSchedule(new Schedule(bookingDetails.Start, bookingDetails.End));

			// update case note
			string note = "";
			if (bookingDetails.Note == null)
				note = $"{client.FirstName} has updated his booking content: From {bookingDetails.Start} to {bookingDetails.End}, {booking.Message}";
			else
			{
				note = $"{client.FirstName} has added a note: {bookingDetails.Note}";
				booking.AddCaseNote(clientID, note);
			}

			// save changes to database
			this._entities.Update(client);
			this._entities.Save();

			// send alert email to the other booking party to let him/her know the booking has changed
			this._emailServices.SendBookingUpdatedByClientEmail(booking, note);
		}

		public void CompleteBooking(int clientID, int bookingID)
		{
			if (clientID < 1)
				throw new ArgumentOutOfRangeException("Value must be an int greater than 1", "clientID");

			if (bookingID < 1)
				throw new ArgumentOutOfRangeException("Value must be an int greater than 1", "bookingID");

			var client = this._entities.Single<Client>(c => c.ID == clientID);
			if (client == null)
				throw new ArgumentNullException("client");

			// get the booking to cancel or do nothing
			var booking = client.GetBooking(bookingID);
			if (booking == null)
				throw new ArgumentNullException("booking");

			// complete booking
			if (booking.Complete())
			{
				// update case note
				var note = $"{client.FirstName} has marked his booking as completed.";
				booking.AddCaseNote(client.ID, note);

				this._entities.Update(client);
				this._entities.Save();

				// send alert email to client
				this._emailServices.SendBookingCompletedEmail(booking, note);
			}
		}

		public UpdateBookingViewModel GetBookingViewModel(int bookingID, int clientID)
		{
			var client = this._entities.Single<Client>(c => c.ID == clientID);
			if (client == null)
				throw new ArgumentNullException("client");

			var booking = client.GetBooking(bookingID);
			if (booking == null)
				throw new ArgumentNullException("booking");

			var careWorkerID = booking.CareWorkerID;
			var careWorker = this._entities.Single<CareWorker>(c => c.ID == careWorkerID);
			if (careWorker == null)
				throw new ArgumentNullException("careWorker");

			var vm = _mapper.Map<Booking, UpdateBookingViewModel>(booking);
			vm.CaseNotes = vm.CaseNotes.AsEnumerable().Reverse().ToList();
			vm.CareWorkerFirstName = careWorker.FirstName;
			vm.OwnerUserID = clientID;
			vm.PreviousBookingID = this._presentationService.GetPreviousBookingID(bookingID);

			if (!string.IsNullOrWhiteSpace(client.Address.FullAddress))
			{
				var clientGeoPoint = new GeoCoordinate((double)client.Address.Latitude, (double)client.Address.Longitude);
				var carerGeoPoint = new GeoCoordinate((double)careWorker.Address.Latitude, (double)careWorker.Address.Longitude);
				var distanceDouble = clientGeoPoint.GetDistanceTo(carerGeoPoint) / 1000;
				var distance = distanceDouble.ToString("F1", CultureInfo.InvariantCulture) + " km";

				vm.Distance = distance;
			}

			return vm;
		}

		public IEnumerable<UpdateBookingViewModel> GetListOfBookingsViewModel(int clientID)
		{
			var client = this._entities.Single<Client>(a => a.ID == clientID);
			if (client == null)
				throw new ArgumentNullException("Client");

			return this._bookingData.GetBookingVMListByClient(client.ID);
		}

		public IEnumerable<NewBookingShortlistViewModel> GetShortlist(int ownerUserID)
		{
			return this._bookingData.GetShortlist(ownerUserID);
		}

		#endregion

		#region Job

		public void CreateNewJob(int clientID, NewJobViewModel model)
		{
			var client = this._entities.Single<Client>(c => c.ID == clientID);
			if (client == null)
				throw new ArgumentNullException("client");
			var job = addNewJob(client.ID, model);
			if (job.Schedules != null)
			{
				job.JobStatus = JobStatus.New;
				client.Jobs.Add(job);
				this._entities.Update(client);
				this._entities.Save();
			}
		}

		public NewJobViewModel GetNewJobViewModel(int clientID, int jobID)
		{
			var client = this._entities.Single<Client>(c => c.ID == clientID);
			if (client == null)
				throw new ArgumentNullException("client");
			var job = client.GetJob(jobID);
			if (job == null)
				return null;
			var model = _mapper.Map<Job, NewJobViewModel>(job);
			model.PatientDropDownList = this._presentationService.GetPatientSelectList(client.ID);
			return model;
		}

		public List<JobsViewModel> GetJobsViewModelList(int clientID)
		{
			var client = this._entities.Single<Client>(c => c.ID == clientID);
			if (client == null)
				throw new ArgumentNullException("client");
			var jobsVM = mapJobsToViewModelList(client);
			return jobsVM;
		}

		public void UpdateJob(int clientID, NewJobViewModel model)
		{
			var client = this._entities.Single<Client>(c => c.ID == clientID);
			if (client == null || client.ID != model.ClientID)
				throw new ArgumentNullException("client");
			var job = client.GetJob(model.ID);
			job = mapViewModelToJob(model, job);
			if (job.Schedules.Count() != 0 && job.Patients.Count() != 0)
			{
				var updateJob = client.UpdateJob(job);
				if (updateJob != null)
				{
					this._entities.Update(client);
					this._entities.Save();
				}
			}
		}

		public bool DeleteJobs(int clientID, string selectedJobsID)
		{
			int count = 0;
			var client = this._entities.Single<Client>(c => c.ID == clientID);
			if (client == null || client.ID != clientID)
				throw new ArgumentNullException("client");
			var selectedJobsIDList = selectedJobsID.Split(',').Select(int.Parse).ToList();
			foreach (var jobID in selectedJobsIDList)
			{
				var job = client.GetJob(jobID);
				if (job != null)
				{
					client.DeleteJob(jobID);
					this._entities.Delete(job);
					this._entities.Update(client);
					this._entities.Save();
					count++;
				}
			}
			if (selectedJobsIDList.Count() == count)
				return true;
			else
				return false;
		}

		public IList<CalendarEventsModel> GetExistingSchedules(int clientID, int jobID)
		{
			var client = this._entities.Single<Client>(c => c.ID == clientID);
			if (client == null || client.ID != clientID)
				throw new ArgumentNullException("client");
			Random random = new Random();
			var startDateTime = DateTime.Now.AddDays(-1);
			var endDateTime = DateTime.Now.AddYears(1);
			var list = new List<CalendarEventsModel>();

			foreach (var jobs in client.Jobs.Where(a => a.JobStatus != JobStatus.Completed || a.JobStatus != JobStatus.Closed
			|| a.JobStatus != JobStatus.Cancelled).Where(a => a.ID != jobID))
			{
				var color = getRandomColor(random); /// "#ffcccc",
				foreach (var schedule in jobs.Schedules)
				{
					var sch = schedule.GetScheduledDurations(startDateTime, endDateTime);
					foreach (var d in sch)
					{
						list.Add(
							new CalendarEventsModel
							{
								ID = schedule.ID,
								Title = jobs.Title,
								StartTime = String.Format("{0:yyyy-MM-dd HH:mm:ss tt}", d.Start),
								EndTime = String.Format("{0:yyyy-MM-dd HH:mm:ss tt}", d.End),
								Location = jobs.Address.FullAddress,
								Color = color
							});
					}
				}
			}
			return list;
		}

		#endregion

		#region Rating

		public void AddRating(int clientID, RatingViewModel ratingData)
		{
			var client = this._entities.Single<Client>(a => a.ID == clientID);
			if (client == null)
				throw new ArgumentNullException("Client");
			var booking = client.GetBooking(ratingData.BookingID);

			Rating rating = _mapper.Map<RatingViewModel, Rating>(ratingData);
			rating.Status = RatingStatus.New;
			booking.AddRating(rating);

			this._entities.Update(client);
			this._entities.Save();

			// send alert email to client
			this._emailServices.SendBookingRatedEmail(booking, $"{client.FirstName} has rated for your a booking.");
		}

		public void UpdateRating(int clientID, double oldRating, RatingViewModel ratingData)
		{
			var client = this._entities.Single<Client>(a => a.ID == clientID);
			if (client == null)
				throw new ArgumentNullException("Client");
			var booking = client.GetBooking(ratingData.BookingID);
			Rating rating = booking.GetRating();
			rating = _mapper.Map<RatingViewModel, Rating>(ratingData, rating);
			rating.Status = RatingStatus.Update;

			booking.UpdateRating(rating);
			this._entities.Update(client);
			this._entities.Save();

			// send alert email to client
			this._emailServices.SendBookingRatingUpdatedEmail(booking, $"{client.FirstName} has updated a rating.");
		}

		public RatingViewModel GetRatingVM(int clientID, int bookingID)
		{
			var client = this._entities.Single<Client>(a => a.ID == clientID);
			if (client == null)
				throw new ArgumentNullException("Client");
			var booking = client.GetBooking(bookingID);
			var rating = booking.GetRating();
			var vm = new RatingViewModel();
			vm = _mapper.Map<Rating, RatingViewModel>(rating);
			return vm;
		}

		#endregion

		#region Heper

		private List<JobsViewModel> mapJobsToViewModelList(Client client)
		{
			var jobs = client.Jobs.OrderByDescending(a => a.CreatedAt);
			var jobsVM = new List<JobsViewModel>();
			jobs.ToList().ForEach(j =>
			{
				jobsVM.Add(mapJobToViewModel(j));
			});
			return jobsVM;
		}

		public JobsViewModel mapJobToViewModel(Job job)
		{
			JobsViewModel model = new JobsViewModel();

			model = _mapper.Map<Job, JobsViewModel>(job);
			if (job.Schedules.Count() != 0)
			{
				var every = "";
				var dayOfWeek = "";
				var endDate = "";
				var byDay = "";
				var schedule = job.Schedules.FirstOrDefault();
				var startTime = schedule.Duration.Start.ToString("HH:mm tt");
				var endTime = schedule.Duration.End.ToString("HH:mm tt");
				if (schedule.EffectiveEndDate != null)
					endDate = String.Format("{0:dddd, MMMM d, yyyy}", schedule.EffectiveEndDate);
				if (schedule.Interval != 1)
				{
					var scheduleType = schedule.ScheduleType.ToString();
					every = "Every " + schedule.Interval + " " + scheduleType.Remove(scheduleType.Length - 2) + "s, on ";
				}

				switch (schedule.ScheduleType)
				{
					case ScheduleType.OneOff:
						byDay = endDate + ", one-off";
						break;
					case ScheduleType.Weekly:
						foreach (var element in job.Schedules)
							dayOfWeek = dayOfWeek + ", " + Enum.GetName(typeof(DayOfWeek), element.GetDayOfWeek());
						if (dayOfWeek != "")
						{
							dayOfWeek = dayOfWeek.TrimStart(',');
							var pos = dayOfWeek.LastIndexOf(',');
							if (pos > 0)
								dayOfWeek = dayOfWeek.Substring(0, pos) + " &" + dayOfWeek.Substring(pos + 1);
						}
						byDay = (endDate != "") ? every + " " + dayOfWeek + ", end on " + endDate : every + " " + dayOfWeek;
						break;
					case ScheduleType.Monthly:
						var day = "on " + Enum.GetName(typeof(FrequencyMonthlyType), schedule.Position);
						if (schedule.Position == 0)
							day = day + " " + schedule.Duration.Start.Day.ToString();
						else
						{
							switch (schedule.ByDay)
							{
								case -1:
									break;
								case 8:
									day = day + " Weekday";
									break;
								case 9:
									day = day + " Weekend day";
									break;
								default:
									day = day + " " + Enum.GetName(typeof(DayOfWeek), schedule.ByDay);
									break;
							}
						}
						byDay = (endDate != "") ? every + " " + day + ", " + schedule.ScheduleType + ", end on " + endDate : every + " " + day + ", " + schedule.ScheduleType;
						break;
				}

				if (schedule.EffectiveEndDate == null)
					byDay = byDay + ", days without end.";

				model.Schedule = startTime + " - " + endTime + ", " + byDay;
				if (schedule.EffectiveEndDate != null)
					model.EndDate = (DateTime)schedule.EffectiveEndDate;
				else
					model.EndDate = DateTime.Now.AddYears(3);
			}

			var patients = "";
			foreach (var element in job.Patients)
				patients = patients + ", " + element.FirstName;
			if (patients != "")
				patients = patients.TrimStart(',');
			model.Patients = patients;
			return model;
		}

		private Job addNewJob(int clientID, NewJobViewModel model)
		{
			var job = _mapper.Map<NewJobViewModel, Job>(model);
			foreach (var patientID in model.PatientIDs)
			{
				var patient = this._entities.Single<Patient>(c => c.ID == patientID);
				if (model.AddressSameProfile)
					job.Address = patient.Address;
				job.AddPatient(patient);
			}
			foreach (var schedule in getJobSchedules(model))
				job.AddSchedule(schedule);
			return job;
		}

		private Job mapViewModelToJob(NewJobViewModel model, Job job)
		{
			job = _mapper.Map<NewJobViewModel, Job>(model, job);
			job.ClearPatients();
			foreach (var patientID in model.PatientIDs)
			{
				var patient = this._entities.Single<Patient>(c => c.ID == patientID);
				if (model.AddressSameProfile)
					job.Address = patient.Address;
				job.AddPatient(patient);
			}
			foreach (Schedule schedule in job.Schedules.ToList())
				this._entities.Delete(schedule);
			job.ClearSchedules();
			foreach (var schedule in getJobSchedules(model))
				job.AddSchedule(schedule);
			return job;
		}

		private List<Schedule> getJobSchedules(NewJobViewModel model)
		{
			DateTime startDateTime = new DateTime();
			DateTime endTime = new DateTime();
			DateTime? endDateTime = new DateTime();
			var schedules = new List<Schedule>();

			int interval = 1;
			int position = 0;
			int byDay = -1;
			if (model.Recurring)
			{
				var dates = model.EventsDate.Split(',');
				if (dates == null)
					return null;
				string startDate = dates.FirstOrDefault();
				startDateTime = Convert.ToDateTime(startDate + " " + model.RecurringStartTime);
				endTime = mergeDateTime(startDateTime, model.RecurringEndTime);
				switch (model.Frequency)
				{
					case ScheduleType.Monthly:
						interval = model.SequenceMonthly;
						position = model.FrequencyMonthly;
						if (position == 5)
							position = -1;
						byDay = model.DayofWeeks;
						break;
					case ScheduleType.Weekly:
						interval = model.SequenceWeekly;
						break;
				}

				var lastDate = dates[dates.Count() - 1];
				foreach (var element in dates.Take(dates.Length - 1))
				{
					startDateTime = Convert.ToDateTime(element + " " + model.RecurringStartTime);
					endTime = mergeDateTime(startDateTime, model.RecurringEndTime);
					switch (model.EndDateOption)
					{
						case "none":
							endDateTime = null;
							break;
						case "after":
							endDateTime = Convert.ToDateTime(lastDate + " " + model.RecurringEndTime);   //need more testing
							break;
						case "endby":
							endDateTime = mergeDateTime((DateTime)model.RecurringEndDate, model.RecurringEndTime);
							break;
					}
					if (model.Frequency == ScheduleType.Weekly)
						byDay = (int)startDateTime.DayOfWeek;

					if (DateTime.Compare(startDateTime, endTime) >= 0)
						endTime = endTime.AddDays(1);
					Schedule schedule = new Schedule(startDateTime, endTime, model.Frequency, interval, position, byDay, endDateTime);
					schedules.Add(schedule);
				}
			}
			else
			{
				startDateTime = mergeDateTime((DateTime)model.StartDate, model.StartTime);
				endTime = mergeDateTime(startDateTime, model.EndTime);
				endDateTime = startDateTime;

				if (DateTime.Compare(startDateTime, endTime) >= 0)
					endTime = endTime.AddDays(1);
				Schedule schedule = new Schedule(startDateTime, endTime, ScheduleType.OneOff, interval, position, byDay, endDateTime);
				schedules.Add(schedule);
			}
			return schedules;
		}

		private DateTime mergeEndDateTime(DateTime date, DateTime time)
		{
			CultureInfo culture = new CultureInfo("en-AU");
			string dateTimeString = date.ToString("d", culture) + " " + time.ToString("hh:mm tt");
			return Convert.ToDateTime(dateTimeString);
		}

		private DateTime mergeDateTime(DateTime startDate, string startTime)
		{
			CultureInfo culture = new CultureInfo("en-AU");
			string dateTimeString = startDate.ToString("d", culture) + " " + startTime;
			return Convert.ToDateTime(dateTimeString);
		}

		// need more testing at here
		private DateTime getEndDate(ScheduleType scheduleType, DateTime startDateTime, int interval, int occurrence)
		{
			//interval==every
			DateTime endDate = new DateTime();
			if (scheduleType == ScheduleType.Weekly)
			{
				var days = (interval * occurrence - interval) * 7;
				endDate = startDateTime.AddDays(days);
			}
			else
			{ //may need change
				var months = interval * occurrence - interval;
				endDate = startDateTime.AddMonths(months);
			}
			return endDate;
		}

		private DateTime getNextWeekday(DateTime start, DayOfWeek day)
		{
			// The (... + 7) % 7 ensures a value in the range [0, 6]
			int daysToAdd = ((int)day - (int)start.DayOfWeek + 7) % 7;
			return start.AddDays(daysToAdd);
		}

		private string getRandomColor(Random random)
		{
			var color = Color.FromArgb(random.Next(200, 254), random.Next(150, 254), random.Next(150, 254));
			var colorString = $"#{color.R:X2}{color.G:X2}{color.B:X2}";
			return colorString;
		}

		#endregion
	}
}