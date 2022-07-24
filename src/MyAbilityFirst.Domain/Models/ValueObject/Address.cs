using System.Data.Entity.Spatial;

namespace MyAbilityFirst.Domain
{
	public class Address
	{
		public string Suburb { get; set; }
		public string State { get; set; }
		public int Postcode { get; set; }
		public string FullAddress { get; set; }
		public decimal Latitude { get; set; }
		public decimal Longitude { get; set; }
		public DbGeography GeoPoint 
		{
			get
			{
				var res = DbGeography.FromText("POINT(" + this.Longitude.ToString() + " " + this.Latitude.ToString() + ")", 4326);
				return res;
			}
			protected set { }
		}
	}
}
