using AutoMapper;
using MyAbilityFirst.Domain;
using MyAbilityFirst.Models;
using MyAbilityFirst.Services.ClientFunctions;
using MyAbilityFirst.Services.Common;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace MyAbilityFirst.Controllers
{
	[Authorize]
	public class MyAccountController : Controller
	{
		#region Fields

		private readonly IUserService _userServices;
		private readonly IClientService _clientServices;
		private readonly IMapper _mapper;

		#endregion

		#region Ctor

		public MyAccountController()
		{

		}

		public MyAccountController(IUserService userService, IClientService clientServices, IMapper mapper)
		{
			this._userServices = userService;
			this._clientServices = clientServices;
			this._mapper = mapper;
		}

		#endregion

		#region Actions

		// GET: MyAccount
		public ActionResult Index()
		{
			string userType = _userServices.GetLoggedInUserType();
			if (userType == "Admin")
			{
				return RedirectToAction("Info", "Manage", new { usertype = userType });
			}
			else
			{
				if (this.GetLoggedInUser().Status == UserStatus.Registered)
				{ // first time login, should redirect to edit profile
					return RedirectToAction("EditProfile", userType, new { usertype = userType });
				}
				else
				{
					return RedirectToAction("MyAccount", userType, new { usertype = userType });
				}
			}
		}

		// GET: MyAccount/Details/5
		[Authorize(Roles = "Admin")]
		public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			User user = this._userServices.GetUser((int)id);
			if (user == null)
			{
				return HttpNotFound();
			}
			return View(user);
		}

		[Authorize]
		[AcceptVerbs(HttpVerbs.Get|HttpVerbs.Post)]
		public ActionResult _MyAccountSidebar()
		{
			var user = this.GetLoggedInUser();
			string loginID = user.LoginIdentityId;
			string userType = _userServices.GetLoggedInUserType();

			MyAccountViewModel vm = new MyAccountViewModel();
			vm.UserType = userType;
			vm.EmailVerified = _userServices.EmailVerified(loginID);

			if (userType == "Client")
			{
				var client = _clientServices.RetrieveClient(this.GetLoggedInUser().ID);
				vm.Bookings = client.GetBookings();
				vm.PatientList = _clientServices.RetrieveAllPatients(client.ID);
			}
			else if (userType == "CareWorker") 
			{
				
			}

			vm.NavSection = string.Format("{0}-{1}", this.ControllerContext.ParentActionViewContext.RouteData.Values["controller"].ToString(), this.ControllerContext.ParentActionViewContext.RouteData.Values["action"].ToString());
			return PartialView("_MyAccountSidebar", vm);
		}

		[AllowAnonymous]
		[AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
		public ActionResult _MyAccountTopbar()
		{
			MyAccountViewModel vm = new MyAccountViewModel();
			if (Request.IsAuthenticated)
			{
				var thisUser = this.GetLoggedInUser();
				if (thisUser != null)
				{ 
					vm.UserName = _userServices.GetUserFirstLastName(thisUser.ID);
				vm.Shortlists = _userServices.RetrieveAllShortlists(thisUser.ID).Where(s => s.Selected).Select(
					sl => {
						ShortlistViewModel vm1 = new ShortlistViewModel();
						var user = _userServices.GetUser(sl.SelectedUserID);
						vm1 = _mapper.Map(user, vm1);
						return vm1;
					}
				).ToList();
				vm.Notifications = _mapper.Map(_userServices.RetrieveOpenNotifications<Notification>(thisUser.ID).ToList().OrderBy(n => n.NotifiedDate), vm.Notifications);
			}
			}
			return PartialView(vm);
		}

		#endregion
	}
}
