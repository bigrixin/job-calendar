using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAbilityFirst.Domain
{
	public class Contact
	{
		public int ID { get; set; }
		public string ContactName { get; set; }
		public string MobilePhone { get; set; }
		public string WorkPhone { get; set; }
		public string HomePhone { get; set; }
		public string Email { get; set; }
		public int RelationshipID { get; set; }
		public int PatientID { get; set; }
	}
}