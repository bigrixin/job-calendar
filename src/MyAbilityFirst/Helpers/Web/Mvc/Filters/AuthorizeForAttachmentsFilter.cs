using MyAbilityFirst.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

public class AuthorizeForAttachmentsFilter : IActionFilter
{

	#region IActionFilter

	public void OnActionExecuting(ActionExecutingContext filterContext)
	{
		// user not logged-in, do nothing
		if (!filterContext.RequestContext.HttpContext.Request.IsAuthenticated)
			return;

		var loggedInUser = (filterContext.Controller as Controller).GetLoggedInUser();

		int relatedUserID;
		int.TryParse(filterContext.ActionParameters["userID"].ToString(), out relatedUserID);

		bool isAuthenticated = false;

		// check if the userID passed via param is the same as the currently logged-in user
		isAuthenticated = loggedInUser.ID == relatedUserID;

		// if not, check if the current logged-in user has access to manage attachments for the userID
		if (!isAuthenticated)
		{
			isAuthenticated = (loggedInUser as Client).Patients.Any(p => p.ID == relatedUserID);

			if (!isAuthenticated)
				filterContext.Result = new RedirectResult("/");
		}
	}

	public void OnActionExecuted(ActionExecutedContext filterContext)
	{
	}

	#endregion

}