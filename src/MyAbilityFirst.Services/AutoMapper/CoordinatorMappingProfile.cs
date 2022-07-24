using AutoMapper;
using MyAbilityFirst.Domain;
using MyAbilityFirst.Models;
using MyAbilityFirst.Services.AttachmentManagement;
using System.Linq;

namespace MyAbilityFirst.Services.Common
{
	public class CoordinatorMappingProfile : Profile
	{

		#region Fields

		private readonly IPresentationService _presentationService;
		private readonly IAttachmentService _attachmentService;

		public override string ProfileName => "CoordinatorMappingProfile";

		#endregion

		#region Ctor

		public CoordinatorMappingProfile(IPresentationService presentationService, IAttachmentService attachmentService)
		{
			this._presentationService = presentationService;
			this._attachmentService = attachmentService;

			this.MapCoordinator();

		}

		#endregion

		#region Helpers


		private void MapCoordinator()
		{
			CreateMap<Coordinator, CoordinatorDetailsViewModel>()
			  .ForMember(dest => dest.GenderDropDownList, opt => opt.MapFrom(src => this._presentationService.GetSubCategorySelectList("Gender")))
					.AfterMap((src, dest) =>
					{
						dest.Gender = dest.GenderDropDownList.SingleOrDefault(list => list.Value == src.GenderID.ToString()) == null ?
						"" : dest.GenderDropDownList.SingleOrDefault(list => list.Value == src.GenderID.ToString()).Text;
					})
				.ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
				.ForMember(dest => dest.OrganisationDropDownList, opt => opt.MapFrom(src => this._presentationService.GetOrganisationList()));

			CreateMap<CoordinatorDetailsViewModel, Coordinator>()
				.ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID));
		}

		#endregion

	}
}
