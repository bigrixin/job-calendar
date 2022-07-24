using MyAbilityFirst.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAbilityFirst.Models
{
	public class MyAccountViewModel
	{
		public string UserType { get; set; }

		// for tracking verified email
		public bool EmailVerified { get; set; }
		public string UserName { get; set; }
		public string NavSection { get; set; }

		// Client Specific
		public ICollection<Patient> PatientList { get; set; }
		public ICollection<Booking> Bookings { get; set; }

		// Shortlists Section
		public ICollection<ShortlistViewModel> Shortlists { get; set; }

		// Notifications
		public List<ChatNotificationViewModel> Notifications { get; set; }
	}
}
