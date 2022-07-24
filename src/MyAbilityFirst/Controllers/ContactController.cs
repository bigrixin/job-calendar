using MyAbilityFirst.Services.ClientFunctions;
using MyAbilityFirst.Services.Common;
using System.Web.Mvc;

namespace MyAbilityFirst.Controllers
{
	public class ContactController : Controller
    {

		#region Fields

		private readonly IPresentationService _viewModelServices;
		private readonly IClientService _clientServices;

		#endregion

		#region Ctor

		public ContactController(IPresentationService viewModelServices, IClientService clientServices)
		{
			this._viewModelServices = viewModelServices;
			this._clientServices = clientServices;
		}

		#endregion

	}
}