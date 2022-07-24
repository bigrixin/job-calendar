using AutoMapper;
using MyAbilityFirst.Domain;
using MyAbilityFirst.Models;
using MyAbilityFirst.Services.Common;
using System.Web.Mvc;

namespace MyAbilityFirst.Controllers
{
	[Authorize]
	public class ShortlistController : Controller
	{

		#region Fields

		private readonly IMapper _mapper;
		private readonly IUserService _userServices;

		#endregion

		#region Ctor

		public ShortlistController(IMapper mapper, IUserService userServices)
		{
			this._mapper = mapper;
			this._userServices = userServices;
		}

		#endregion

		#region Actions

		[HttpGet]
		public ActionResult ShortlistButton(int selectedUserID)
		{
			ShortlistViewModel vm = new ShortlistViewModel();
			var user = this.GetLoggedInUser();
			if (user == null) 
			{
				return PartialView(vm);
			}
			Shortlist shortlist = _userServices.RetrieveShortlistBySelectedUserID(user.ID, selectedUserID);
			vm = _mapper.Map<Shortlist, ShortlistViewModel>(shortlist, vm) ?? new ShortlistViewModel();
			vm.SelectedUserID = shortlist == null ? selectedUserID : shortlist.SelectedUserID;
			return PartialView(vm);
		}


		[HttpPost]
		public ActionResult UpdateShortlist(ShortlistViewModel vm)
		{
			if (ModelState.IsValid)
			{
				int ownerUserID = this.GetLoggedInUser().ID;
				Shortlist shortlist = _userServices.RetrieveShortlistBySelectedUserID(ownerUserID, vm.SelectedUserID);
				if (shortlist == null) 
				{
					return AddShortlist(vm);
				}
				shortlist = _mapper.Map<ShortlistViewModel, Shortlist>(vm, shortlist);
				_userServices.UpdateShortlist(ownerUserID, shortlist);
			}
			return Json(vm);
		}

		[HttpPost]
		public ActionResult AddShortlist(ShortlistViewModel vm)
		{
			if (ModelState.IsValid)
			{
				int ownerUserID = this.GetLoggedInUser().ID;
				Shortlist shortlist = new Shortlist(ownerUserID, vm.SelectedUserID, vm.Selected);
				shortlist = _mapper.Map<ShortlistViewModel, Shortlist>(vm, shortlist);
				_userServices.CreateShortlist(ownerUserID, shortlist);
			}
			return Json(vm);
		}

		#endregion

	}
}