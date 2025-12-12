using BellaFrisoer.Domain.Models;
using BellaFrisoer.Domain.Models.Discounts;
using System;

namespace BellaFrisoer.Application.Interfaces
{
    public interface IBookingPriceService
    {
        decimal CalculateFinalPrice(
            Booking booking,
            Employee? employee,
            Treatment? treatment,
            Customer? customer,
            bool eventEnabled,
            DateTime? eventStartDate,
            DateTime? eventEndDate,
            TimeOnly? eventStartTime,
            TimeOnly? eventEndTime,
            decimal? eventPercent);
    }
}
