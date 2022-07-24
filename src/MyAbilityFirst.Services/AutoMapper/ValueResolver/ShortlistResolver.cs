using AutoMapper;
using MyAbilityFirst.Domain;
using MyAbilityFirst.Domain.ClientFunctions;
using MyAbilityFirst.Infrastructure.Data;
using MyAbilityFirst.Services.ClientFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MyAbilityFirst.Services.Common
{
	public class ShortlistResolver : IValueResolver<Booking, UpdateBookingViewModel, IEnumerable<SelectListItem>>
	{

		#region Fields

		private readonly BookingData _bookingData;

		#endregion

		#region Ctor

		public ShortlistResolver(BookingData bookingData)
		{
			this._bookingData = bookingData;
		}

		#endregion

		#region Helpers

		public IEnumerable<SelectListItem> Resolve(Booking source, UpdateBookingViewModel destination, IEnumerable<SelectListItem> member, ResolutionContext context)
		{
			var shortlists =
				this._bookingData
						.GetShortlist(source.ClientID)
						.Select(s => new SelectListItem {
														Value = s.CareWorkerID.ToString(),
														Text = s.CareWorkerFirstName
													});

			return shortlists;
		}

		#endregion

	}
}