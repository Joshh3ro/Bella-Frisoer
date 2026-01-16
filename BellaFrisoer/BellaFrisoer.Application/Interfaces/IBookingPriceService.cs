using BellaFrisoer.Domain.Models;
using BellaFrisoer.Domain.Models.Discounts;
using System;

namespace BellaFrisoer.Application.Interfaces
{
    public interface IBookingPriceService
    {
        Task<decimal> CalculateFinalPrice(Booking booking, Customer customer);

    }
}