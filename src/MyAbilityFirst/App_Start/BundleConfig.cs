using System.Web.Optimization;
using MyAbilityFirst.Constants;

namespace MyAbilityFirst
{
	public static class BundleConfig
	{

		/// <summary>
		/// For more information on bundling, visit <see cref="http://go.microsoft.com/fwlink/?LinkId=301862"/>.
		/// </summary>
		public static void RegisterBundles(BundleCollection bundles)
		{
			// Enable Optimizations
			// Set EnableOptimizations to false for debugging. For more information,
			// Web.config file system.web/compilation[debug=true]
			// OR
			// BundleTable.EnableOptimizations = true;

			// Enable CDN usage.
			// Note: that you can choose to remove the CDN if you are developing an intranet application.
			// Note: We are using Google's CDN where possible and then Microsoft if not available for better
			//       performance (Google is more likely to have been cached by the users browser).
			// Note: that protocol (http:) is omitted from the CDN URL on purpose to allow the browser to choose the protocol.
			bundles.UseCdn = true;

			AddCss(bundles);
			AddJavaScript(bundles);
		}

		private static void AddCss(BundleCollection bundles)
		{
			// Bootstrap - Twitter Bootstrap CSS (http://getbootstrap.com/).
			// Site - Your custom site CSS.
			// Note: No CDN support has been added here. Most likely you will want to customize your copy of bootstrap.
			bundles.Add(new StyleBundle(
					"~/Content/css")
					.Include("~/Content/bootstrap/site.css")
					.Include("~/Content/site.css")
					.Include("~/Content/themes/base/jquery-ui.css"));

			// Bootstrap datetime picker CSS
			bundles.Add(new StyleBundle(
					"~/Content/bootstrap-datetimepicker")
					.Include("~/Content/bootstrap-datetimepicker.min.css"));

			// Bootstrap tags input CSS
			bundles.Add(new StyleBundle(
					"~/Content/bootstrap-tagsinput")
					.Include("~/Content/bootstrap-tagsinput.css"));

			// Bootstrap toggle CSS
			bundles.Add(new StyleBundle(
					"~/Content/bsToggle")
					.Include("~/Content/bootstrap-toggle.min.css"));

			// Bootstrap toggle CSS
			bundles.Add(new StyleBundle(
					"~/Content/bsNavSteps")
					.Include("~/Content/bootstrap-nav-wizard.min.css"));

			// Font Awesome - Icons using font (http://fortawesome.github.io/Font-Awesome/).
			bundles.Add(new StyleBundle(
					"~/Content/fa",
					ContentDeliveryNetwork.MaxCdn.FontAwesomeUrl)
					.Include("~/Content/fontawesome/site.css"));

			// Calendar CSS
			bundles.Add(new StyleBundle(
					"~/Content/fullCalendar")
					.Include("~/Content/fullcalendar.min.css"));
				//	.Include("~/Content/fullcalendar.print.css"));
		}

		/// <summary>
		/// Creates and adds JavaScript bundles to the bundle collection. Content Delivery Network's (CDN) are used
		/// where available.
		///
		/// Note: MVC's built in <see cref="System.Web.Optimization.Bundle.CdnFallbackExpression"/> is not used as
		/// using in-line scripts is not permitted under Content Security Policy (CSP) (See <see cref="FilterConfig"/>
		/// for more details).
		///
		/// Instead, we create our own fail-over bundles. If a CDN is not reachable, the fail-over script loads the
		/// local bundles instead. The fail-over script is only a few lines of code and should have a minimal impact,
		/// although it does add an extra request (Two if the browser is IE8 or less). If you feel confident in the CDN
		/// availability and prefer better performance, you can delete these lines.
		/// </summary>
		/// <param name="bundles">The bundles.</param>
		private static void AddJavaScript(BundleCollection bundles)
		{
			// jQuery - The JavaScript helper API (http://jquery.com/).
			Bundle jqueryBundle = new ScriptBundle("~/bundles/jquery", ContentDeliveryNetwork.Google.JQueryUrl)
					.Include("~/Scripts/jquery-{version}.js");
			bundles.Add(jqueryBundle);

			// jQuery Validate - Client side JavaScript form validation (http://jqueryvalidation.org/).
			Bundle jqueryValidateBundle = new ScriptBundle(
					"~/bundles/jqueryval",
					ContentDeliveryNetwork.Microsoft.JQueryValidateUrl)
					.Include("~/Scripts/jquery.validate*");
			bundles.Add(jqueryValidateBundle);

			// Microsoft jQuery Validate Unobtrusive - Validation using HTML data- attributes
			// http://stackoverflow.com/questions/11534910/what-is-jquery-unobtrusive-validation
			Bundle jqueryValidateUnobtrusiveBundle = new ScriptBundle(
					"~/bundles/jqueryvalunobtrusive",
					ContentDeliveryNetwork.Microsoft.JQueryValidateUnobtrusiveUrl)
					.Include("~/Scripts/jquery.validate*");
			bundles.Add(jqueryValidateUnobtrusiveBundle);

			// jQuery-UI
			Bundle jqueryUIBundle = new ScriptBundle("~/bundles/jqueryui", ContentDeliveryNetwork.Microsoft.JQueryValidateUrl)
						.Include("~/Scripts/jquery-ui-{version}.js");
			bundles.Add(jqueryUIBundle);

			// Modernizr - Allows you to check if a particular API is available in the browser (http://modernizr.com).
			// Use the development version of Modernizr to develop with and learn from. Then, when you're
			// ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
			// Note: The current version of Modernizr does not support Content Security Policy (CSP) (See FilterConfig).
			// See here for details: https://github.com/Modernizr/Modernizr/pull/1263 and
			// http://stackoverflow.com/questions/26532234/modernizr-causes-content-security-policy-csp-violation-errors
			Bundle modernizrBundle = new ScriptBundle(
																	"~/bundles/modernizr",
																	ContentDeliveryNetwork.Microsoft.ModernizrUrl)
																	.Include("~/Scripts/modernizr-*");
			bundles.Add(modernizrBundle);

			// Bootstrap - Twitter Bootstrap JavaScript (http://getbootstrap.com/).
			Bundle bootstrapBundle = new ScriptBundle(
																	"~/bundles/bootstrap",
																	ContentDeliveryNetwork.Microsoft.BootstrapUrl)
																	.Include("~/Scripts/bootstrap.js");
			bundles.Add(bootstrapBundle);

			// Bootstrap datetimepicker
			Bundle bsDatepickerBundle = new ScriptBundle("~/bundles/bootstrap-datetimepicker")
							.Include("~/Scripts/bootstrap-datetimepicker.min.js");
			bundles.Add(bsDatepickerBundle);


			// Bootstrap tagsinput
			Bundle bsTagsinputBundle = new ScriptBundle("~/bundles/bootstrap-tagsinput")
							.Include("~/Scripts/bootstrap-tagsinput.js");
			bundles.Add(bsTagsinputBundle);

			// Bootstrap toggle
			Bundle bsToggleBundle = new ScriptBundle("~/bundles/bsToggle")
							.Include("~/Scripts/bootstrap-toggle.min.js");
			bundles.Add(bsToggleBundle);

			// Geocomplete 
			Bundle geocompleteBundle = new ScriptBundle("~/bundles/jquery-geocomplete")
							.Include("~/Scripts/jquery.geocomplete.min.js");
			bundles.Add(geocompleteBundle);

			// jQuery SignalR 
			Bundle signalRBundle = new ScriptBundle("~/bundles/jquery-signalR")
							.Include("~/Scripts/jquery.signalR-2.2.2.min.js");
			bundles.Add(signalRBundle);

			// Moment JS
			Bundle momentBundle = new ScriptBundle("~/bundles/moment")
							.Include("~/Scripts/moment.min.js");
			bundles.Add(momentBundle);

			// Calendar JS
			Bundle calendarBundle = new ScriptBundle("~/bundles/fullCalendar")
							.Include("~/Scripts/fullcalendar.min.js");
			bundles.Add(calendarBundle);

			// Script bundle for the site. The fall-back scripts are for when a CDN fails, in this case we load a local
			// copy of the script instead.
			Bundle failoverCoreBundle = new ScriptBundle("~/bundles/site")
					.Include("~/Scripts/Fallback/styles.js")
					.Include("~/Scripts/Fallback/scripts.js")
					.Include("~/Scripts/site.js");
			bundles.Add(failoverCoreBundle);
		}

	}
}