using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAbilityFirst.Models
{
	public class ShortlistViewModel
	{
		public int SelectedUserID { get; set; }
		public bool Selected { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string PictureURL { get; set; }
	}
}