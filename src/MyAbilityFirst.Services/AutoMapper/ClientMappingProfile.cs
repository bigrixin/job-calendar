using AutoMapper;
using MyAbilityFirst.Domain;
using MyAbilityFirst.Domain.ClientFunctions;
using MyAbilityFirst.Models;
using MyAbilityFirst.Services.AttachmentManagement;
using System;
using System.Collections.Generic;
using System.Linq;


namespace MyAbilityFirst.Services.Common
{
	public class ClientMappingProfile : Profile
	{

		#region Fields

		private readonly IPresentationService _presentationService;
		private readonly IAttachmentService _attachmentService;

		public override string ProfileName => "ClientMappingProfile";

		#endregion

		#region Ctor

		public ClientMappingProfile(IPresentationService presentationService, IAttachmentService attachmentService)
		{
			this._presentationService = presentationService;
			this._attachmentService = attachmentService;

			this.MapClient();
			this.MapPatient();
			this.MapContact();
			this.MapBooking();
			this.MapJob();
			this.MapRating();
		}

		#endregion

		#region Helpers

		private void MapClient()
		{
			CreateMap<Client, ClientDetailsViewModel>()
				.ForMember(dest => dest.ClientID, opt => opt.MapFrom(src => src.ID))
				.ForMember(dest => dest.GenderDropDownList, opt => opt.MapFrom(src => this._presentationService.GetSubCategorySelectList("Gender")))
					.AfterMap((src, dest) =>
					{
						dest.Gender = dest.GenderDropDownList.SingleOrDefault(list => list.Value == src.GenderID.ToString()) == null ?
						"" : dest.GenderDropDownList.SingleOrDefault(list => list.Value == src.GenderID.ToString()).Text;
					})
				.ForMember(dest => dest.MarketingInfoList, opt => opt.MapFrom(src => this._presentationService.GetSubCategoryList("ClientMarketingInfo")))
				.ForMember(dest => dest.PreviousMarketingInfo, opt => opt.MapFrom(src => this._presentationService.GetSubCategoryListByUser("ClientMarketingInfo", src.ID)));

			CreateMap<ClientDetailsViewModel, Client>()
				.IncludeBase<UserDetailsViewModel, User>()
				.ForMember(dest => dest.Patients, opt => opt.Ignore())
				.ForMember(dest => dest.Jobs, opt => opt.Ignore());
		}

		private void MapPatient()
		{
			CreateMap<Patient, PatientDetailsViewModel>()
				.ForMember(dest => dest.PatientID, opt => opt.MapFrom(src => src.ID))
				.ForMember(dest => dest.GenderDropDownList, opt => opt.MapFrom(src => this._presentationService.GetSubCategorySelectList("Gender")))
					.AfterMap((src, dest) =>
					{
						dest.Gender = dest.GenderDropDownList.SingleOrDefault(list => list.Value == src.GenderID.ToString()) == null ?
						"" : dest.GenderDropDownList.SingleOrDefault(list => list.Value == src.GenderID.ToString()).Text;
					})
				.ForMember(dest => dest.LanguageDropDownList, opt => opt.MapFrom(src => this._presentationService.GetSubCategorySelectList("Language")))
					.AfterMap((src, dest) =>
					{
						dest.FirstLanguage = dest.LanguageDropDownList.SingleOrDefault(list => list.Value == src.FirstLanguageID.ToString()) == null ?
						"" : dest.LanguageDropDownList.SingleOrDefault(list => list.Value == src.FirstLanguageID.ToString()).Text;
					})
					.AfterMap((src, dest) =>
					{
						dest.SecondLanguage = dest.LanguageDropDownList.SingleOrDefault(list => list.Value == src.SecondLanguageID.ToString()) == null ?
						"" : dest.LanguageDropDownList.SingleOrDefault(list => list.Value == src.SecondLanguageID.ToString()).Text;
					})
				.ForMember(dest => dest.CultureDropDownList, opt => opt.MapFrom(src => this._presentationService.GetSubCategorySelectList("Culture")))
					.AfterMap((src, dest) =>
					{
						dest.Culture = dest.CultureDropDownList.SingleOrDefault(list => list.Value == src.CultureID.ToString()) == null ?
						"" : dest.CultureDropDownList.SingleOrDefault(list => list.Value == src.CultureID.ToString()).Text;
					})
				.ForMember(dest => dest.ReligionDropDownList, opt => opt.MapFrom(src => this._presentationService.GetSubCategorySelectList("Religion")))
					.AfterMap((src, dest) =>
					{
						dest.Religion = dest.ReligionDropDownList.SingleOrDefault(list => list.Value == src.ReligionID.ToString()) == null ?
						"" : dest.ReligionDropDownList.SingleOrDefault(list => list.Value == src.ReligionID.ToString()).Text;
					})
				.ForMember(dest => dest.CareTypeDropDownList, opt => opt.MapFrom(src => this._presentationService.GetSubCategorySelectList("CareType")))
					.AfterMap((src, dest) =>
					{
						dest.CareType = dest.CareTypeDropDownList.SingleOrDefault(list => list.Value == src.CareTypeID.ToString()) == null ?
						"" : dest.CareTypeDropDownList.SingleOrDefault(list => list.Value == src.CareTypeID.ToString()).Text;
					})
				.ForMember(dest => dest.RelationshipDropDownList, opt => opt.MapFrom(src => this._presentationService.GetSubCategorySelectList("Relationship")))
				.ForMember(dest => dest.PetDropDownList, opt => opt.MapFrom(src => this._presentationService.GetSubCategorySelectList("Pet")))
				.ForMember(dest => dest.InterestList, opt => opt.MapFrom(src => this._presentationService.GetSubCategoryList("Interest")))
				.ForMember(dest => dest.MedicalLivingNeedsList, opt => opt.MapFrom(src => this._presentationService.GetSubCategoryList("MedicalLivingNeed")))
				.ForMember(dest => dest.PreviousInterestList, opt => opt.MapFrom(src => this._presentationService.GetSubCategoryListByUser("Interest", src.ID)))
				.ForMember(dest => dest.PreviousMedicalLivingNeedsList, opt => opt.MapFrom(src => this._presentationService.GetSubCategoryListByUser("MedicalLivingNeed", src.ID)))
				.ForMember(dest => dest.Contacts, opt => opt.MapFrom(src => src.Contacts));

			CreateMap<PatientDetailsViewModel, Patient>()
				.ForMember(dest => dest.Contacts, opt => opt.Ignore());

			CreateMap<Patient, PatientAttachmentViewModel>()
				.ForMember(dest => dest.PatientID, opt => opt.MapFrom(src => src.ID))
				.ForMember(dest => dest.AttachmentList, opt => opt.MapFrom(src => this._presentationService.GetAttachmentList()))
				.ForMember(dest => dest.PreviousAttachmentList, opt => opt.MapFrom(src => this._attachmentService.GetAttachmentsForUser(src.ID)));
		}

		private void MapContact()
		{
			CreateMap<Contact, Contact>();

			CreateMap<List<Contact>, List<Contact>>();
		}

		private void MapBooking()
		{
			CreateMap<Booking, UpdateBookingViewModel>()
				.ForMember(dest => dest.BookingID, opt => opt.MapFrom(src => src.ID))
				.ForMember(dest => dest.Shortlist, opt => opt.ResolveUsing<ShortlistResolver>())
				.ForMember(dest => dest.Start, opt => opt.MapFrom(src => src.Schedule.Duration.Start))
				.ForMember(dest => dest.End, opt => opt.MapFrom(src => src.Schedule.Duration.End))
				.ForMember(dest => dest.IsCancelled, opt => opt.MapFrom(src => src.Status == BookingStatus.Cancelled))
				.ForMember(dest => dest.IsLapsed, opt => opt.MapFrom(src => src.Schedule.Duration.End < DateTime.Now));
		}

		private void MapJob()
		{
			CreateMap<Job, JobViewModel>()
				.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ID))
				//.ForMember(dest => dest.PatientId, opt => opt.MapFrom(src => src.PatientId))
				//.ForMember(dest => dest.ServicedAt, opt => opt.MapFrom(src => src.ServiceAt))
				.ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
				.ForMember(dest => dest.GenderDropDownList, opt => opt.MapFrom(src => this._presentationService.GetSubCategorySelectList("Gender")))
				.ForMember(dest => dest.ServiceDropDownList, opt => opt.MapFrom(src => this._presentationService.GetSubCategorySelectList("JobService")))
				.ForMember(dest => dest.PatientDropDownList, opt => opt.MapFrom(src => this._presentationService.GetPatientSelectList(src.ClientID)));

			CreateMap<JobViewModel, Job>()
				.ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.Id));
			//	.ForMember(dest => dest.PatientId, opt => opt.MapFrom(src => src.PatientId))
			//	.ForMember(dest => dest.ServiceAt, opt => opt.MapFrom(src => src.ServicedAt));


			//new version
			CreateMap<Job, NewJobViewModel>()
			.ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
			.ForMember(dest => dest.ClientID, opt => opt.MapFrom(src => src.ClientID))
			.ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
			.ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
			.ForMember(dest => dest.PreferredGenderID, opt => opt.MapFrom(src => src.PreferredGenderID))
			.ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
			.ForMember(dest => dest.PatientIDs, opt => opt.MapFrom(src => src.Patients.Select(p => p.ID).ToList()))
			.ForMember(dest => dest.SelectedPatients, opt => opt.MapFrom(src => this._presentationService.GetSelectedPatients(src.Patients.ToList(), src.ClientID)))
			.ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.Schedules.FirstOrDefault().Duration.Start))
			.ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => String.Format("{0:t}", src.Schedules.FirstOrDefault().Duration.Start)))
			.ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => String.Format("{0:t}", src.Schedules.FirstOrDefault().Duration.End)))
			.ForMember(dest => dest.RecurringStartDate, opt => opt.MapFrom(src => src.Schedules.FirstOrDefault().Duration.Start))
			.ForMember(dest => dest.RecurringEndDate, opt => opt.MapFrom(src => src.Schedules.FirstOrDefault().EffectiveEndDate != null ? src.Schedules.FirstOrDefault().EffectiveEndDate : (DateTime?)null))
			.ForMember(dest => dest.RecurringStartTime, opt => opt.MapFrom(src => String.Format("{0:t}", src.Schedules.FirstOrDefault().Duration.Start)))
			.ForMember(dest => dest.RecurringEndTime, opt => opt.MapFrom(src => String.Format("{0:t}", src.Schedules.FirstOrDefault().Duration.End)))
			.ForMember(dest => dest.Recurring, opt => opt.MapFrom(src => src.Schedules.FirstOrDefault().ScheduleType == ScheduleType.OneOff ? false : true))
			.ForMember(dest => dest.Frequency, opt => opt.MapFrom(src => src.Schedules.FirstOrDefault().ScheduleType))
			.ForMember(dest => dest.SequenceMonthly, opt => opt.MapFrom(src => src.Schedules.FirstOrDefault().ScheduleType == ScheduleType.Monthly ? src.Schedules.FirstOrDefault().Interval : 1))
			.ForMember(dest => dest.SequenceWeekly, opt => opt.MapFrom(src => src.Schedules.FirstOrDefault().ScheduleType == ScheduleType.Weekly ? src.Schedules.FirstOrDefault().Interval : 1))
			.ForMember(dest => dest.FrequencyMonthly, opt => opt.MapFrom(src => src.Schedules.FirstOrDefault().Position == -1 ? 5 : src.Schedules.FirstOrDefault().Position))
			.ForMember(dest => dest.Day, opt => opt.MapFrom(src => src.Schedules.FirstOrDefault().Duration.Start.Day))
			.ForMember(dest => dest.DayofWeeks, opt => opt.MapFrom(src => src.Schedules.FirstOrDefault().ByDay))
			.ForMember(dest => dest.SelectedDayofWeek, opt => opt.MapFrom(src => this._presentationService.GetSelectedDayofWeek(src.Schedules.ToList())))
			.ForMember(dest => dest.EndDateOption, opt => opt.MapFrom(src => src.Schedules.FirstOrDefault().EffectiveEndDate == null ? "none" : "endby"))
			.ForMember(dest => dest.GenderDropDownList, opt => opt.MapFrom(src => this._presentationService.GetSubCategorySelectList("Gender")));

			CreateMap<NewJobViewModel, Job>()
			 .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
			 .ForMember(dest => dest.ClientID, opt => opt.MapFrom(src => src.ClientID))
			 .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
			 .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
			 .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
			 .ForMember(dest => dest.PreferredGenderID, opt => opt.MapFrom(src => src.PreferredGenderID))
			 .ForMember(dest => dest.Patients, opt => opt.Ignore())
			 .ForMember(dest => dest.Schedules, opt => opt.Ignore());


			CreateMap<Job, JobsViewModel>()
			.ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
			.ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
			.ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
			.ForMember(dest => dest.JobStatus, opt => opt.MapFrom(src => src.JobStatus))
			.ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
			.ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));

		}

		private void MapRating()
		{
			CreateMap<Rating, RatingViewModel>()
			 .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID));

			CreateMap<RatingViewModel, Rating>()
			 .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID));
		}

		#endregion

	}
}
