using MyAbilityFirst.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAbilityFirst.Models
{
	public class UserDetailsViewModel
	{
		// User
		[Required]
		public string FirstName { get; set; }

		[Required]
		public string LastName { get; set; }

		[Required]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy}")]
		public DateTime? DoB { get; set; }

		public int GenderID { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; }

		public string Phone { get; set; }

		public Address Address { get; set; }
	}
}