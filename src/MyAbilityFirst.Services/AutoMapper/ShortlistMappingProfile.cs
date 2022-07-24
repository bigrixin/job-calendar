using AutoMapper;
using MyAbilityFirst.Domain;
using MyAbilityFirst.Models;

namespace MyAbilityFirst.Services.Common
{
	public class ShortlistMappingProfile : Profile
	{

		#region Fields
		public override string ProfileName => "ShortlistMappingProfile";
		#endregion

		#region Ctor

		public ShortlistMappingProfile()
		{
			this.MapUserCriteria();
			this.MapShortlistVMCriteria();
		}

		#endregion

		#region Helpers

		private void MapUserCriteria()
		{
			CreateMap<User, ShortlistViewModel>()
				.ForMember(dest => dest.SelectedUserID, opt => opt.MapFrom(src => src.ID))
				.ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
				.ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
				.ForMember(dest => dest.PictureURL, opt => opt.MapFrom(src => "https://www.w3schools.com/bootstrap/img_avatar3.png"))
				.ForMember(dest => dest.Selected, opt => opt.MapFrom(src => true));
		}


		private void MapShortlistVMCriteria()
		{
			CreateMap<Shortlist, ShortlistViewModel>();

			CreateMap<ShortlistViewModel, Shortlist>()
				.ForMember(dest => dest.ID, opt => opt.Ignore());

			CreateMap<Shortlist, Shortlist>();
		}

		#endregion
	}
}