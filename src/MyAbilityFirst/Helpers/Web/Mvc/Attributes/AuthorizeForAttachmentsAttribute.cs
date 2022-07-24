using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public class AuthorizeForAttachmentsAttribute : ActionFilterAttribute
{

	#region OnActionExecuting

	public override void OnActionExecuting(ActionExecutingContext filterContext)
	{
		var filter = new AuthorizeForAttachmentsFilter();
		filter.OnActionExecuting(filterContext);
	}

	#endregion

}
