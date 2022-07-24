using System;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Linq;

namespace MyAbilityFirst.Infrastructure.Auth
{
	public class LoginService : ILoginService
	{
		private readonly UserManager<IdentityUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;

		public LoginService(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
		{
			_userManager = userManager;
			_roleManager = roleManager;
		}

		public Task SignOn()
		{
			return null;
		}
		public Task SignOut()
		{
			return null;
		}

		public string GetCurrentLoginIdentityID()
		{
			return HttpContext.Current.User.Identity.GetUserId();
		}

		public bool EmailVerified(string loginIdentityID)
		{
			return _userManager.IsEmailConfirmed(loginIdentityID);
		}

		public string GetCurrentLoginUserName()
		{
			return HttpContext.Current.User.Identity.GetUserName();
		}

		public string GetUserType(string loginIdentityID)
		{
			var user = this._userManager.FindById(loginIdentityID);
			if (user != null && user.Roles.Count > 0)
			{
				// Returning first role found
				var roleId = user.Roles.First().RoleId;
				return this._roleManager.FindById(roleId).Name;
			}
			return "";
		}

		public bool IsAuthenticated()
		{
			return HttpContext.Current.Request.IsAuthenticated;
		}
		
		public string GetLoginUserName(string loginIdentityID)
		{
			var user = this._userManager.FindById(loginIdentityID);
			if (user != null)
				return user.UserName;
			return null;
		}
	}
}

