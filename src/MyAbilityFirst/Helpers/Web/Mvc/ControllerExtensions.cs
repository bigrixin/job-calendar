using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MyAbilityFirst.Domain;
using MyAbilityFirst.Infrastructure;
using MyAbilityFirst.Infrastructure.Auth;
using System.Web.Security;
using System.Security.Claims;
using System.IO;
using MyAbilityFirst.Services.Common;

public static class ControllerExtensions
{

	#region Extensions

	public static User GetLoggedInUser(this Controller c)
	{
		return DependencyResolver.Current.GetService<IUserService>().GetLoggedInUser();
	}

	/* Returns HTML string based on partial view
	 * https://stackoverflow.com/questions/1471066/partial-views-vs-json-or-both 
	 */
	public static string RenderViewToString(this Controller controller, string viewName, object model)
	{
		using (var writer = new StringWriter())
		{
			var viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
			controller.ViewData.Model = model;
			var viewCxt = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, writer);
			viewCxt.View.Render(viewCxt, writer);
			return writer.ToString();
		}
	}

	#endregion

}
