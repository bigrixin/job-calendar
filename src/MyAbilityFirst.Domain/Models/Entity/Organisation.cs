

namespace MyAbilityFirst.Domain
{
	public class Organisation
	{
		#region Properties

		public int ID { get; set; }
		public string Name { get; set; }
		public string LogoURL { get; set; }
		public string Address { get; set; }
		public string Phone { get; set; }
		public string Introduction { get; set; }

		#endregion

		#region Ctor

		protected Organisation()
		{
			// Required by EF
		}

		#endregion
	}
}
