using MyAbilityFirst.Domain;
using System;

namespace MyAbilityFirst.Models
{
	public class CareWorkerSearchResult
	{
		public int CareWorkerID { get; set; }
		public Address Address { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Description { get; set; }
		public double? DistanceFromSearchInKm { get; set; }
		public bool WithinServiceCoverage { get; set; }
		public decimal? Longitude { get; set; }
		public decimal? Latitude { get; set; }
		public bool? Shortlisted { get; set; }
		public string PictureURL { get; set; }
	}
}
