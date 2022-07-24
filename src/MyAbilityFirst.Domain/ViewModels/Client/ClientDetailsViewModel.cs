using MyAbilityFirst.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MyAbilityFirst.Models
{
	public class ClientDetailsViewModel : UserDetailsViewModel
	{

		public int ClientID { get; set; }
		public NotificationSettings NotificationSettings { get; set; }
		public Disclaimers Disclaimers { get; set; }

		// View Texts
		public string Gender { get; set; }

		// single select dropdowns
		public SelectList GenderDropDownList { get; set; }

		// multi selects
		public List<Subcategory> MarketingInfoList { get; set; }

		// for tracking changes
		public List<Subcategory> PreviousMarketingInfo { get; set; }
		public int[] PostedSubCategoryIDs { get; set; }

	}
}