using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

public static class HtmlExtensions
{

	public static IHtmlString TextBoxFor<TModel, TProperty>(
			this HtmlHelper<TModel> htmlHelper,
			Expression<Func<TModel, TProperty>> expression,
			object htmlAttributes,
			bool disabled
	)
	{
		var attributes = new RouteValueDictionary(htmlAttributes);
		if (disabled)
		{
			attributes["disabled"] = "disabled";
		}

		return htmlHelper.TextBoxFor(expression, attributes);
	}

	public static IHtmlString DropDownListFor<TModel, TProperty>(
		this HtmlHelper<TModel> htmlHelper,
		Expression<Func<TModel, TProperty>> expression,
		IEnumerable<SelectListItem> selectList,
		object htmlAttributes,
		bool disabled
)
	{
		var attributes = new RouteValueDictionary(htmlAttributes);
		if (disabled)
		{
			attributes["disabled"] = "disabled";
		}

		return htmlHelper.DropDownListFor(expression, selectList, attributes);
	}
}