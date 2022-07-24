using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAbilityFirst.Infrastructure.Auth
{
	public interface ILoginService
	{
		Task SignOn();
		Task SignOut();
		string GetCurrentLoginIdentityID();
		bool EmailVerified(string loginIdentityID);
		string GetCurrentLoginUserName();
		string GetUserType(string loginIdentityID);
		bool IsAuthenticated();
		string GetLoginUserName(string loginIdentityID);
	}
}