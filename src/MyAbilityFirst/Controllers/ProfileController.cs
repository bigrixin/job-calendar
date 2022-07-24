using AutoMapper;
using MyAbilityFirst.Domain;
using MyAbilityFirst.Models;
using MyAbilityFirst.Services.CareWorkerFunctions;
using System;
using System.Web.Mvc;

namespace MyAbilityFirst.Controllers
{
	[Authorize]
	public class ProfileController : Controller
	{
		private readonly ICareWorkerService _careWorkerServices;
		private readonly IMapper _mapper;

		public ProfileController(ICareWorkerService careWorkerServices, IMapper mapper)
		{
			_careWorkerServices = careWorkerServices;
			_mapper = mapper;
		}
		
		[HttpGet]
		public ActionResult PublicProfile(int id)
		{
			CareWorkerPublicProfileViewModel vm = new CareWorkerPublicProfileViewModel();
			CareWorker careworker = _careWorkerServices.RetrieveCareWorker(id);

			if (careworker != null && careworker.Status == UserStatus.Active)
			{
				vm = _mapper.Map(careworker, vm);
				vm.CareWorkerID = id;
				vm.OverallRating = Math.Round(careworker.TotalRating / careworker.RatingCount, 1);
				return View(vm);
			}
			else
			{
				return RedirectToAction("Index", "Home");
			}
		}

		[Authorize(Roles = "Coordinator")]
		[HttpGet, Route("profile/careworkerprofile/{CareWorkerID:int}")]
		public ActionResult CareWorkerProfile(int careWorkerID)
		{
			CareWorkerDetailsViewModel vm = new CareWorkerDetailsViewModel();
			CareWorker careworker = _careWorkerServices.RetrieveCareWorker(careWorkerID);
			vm = _mapper.Map(careworker, vm);
			return View(vm);
		}
	}
}