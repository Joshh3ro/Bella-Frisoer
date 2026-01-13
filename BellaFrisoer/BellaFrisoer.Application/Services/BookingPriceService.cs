using BellaFrisoer.Application.Interfaces;
using BellaFrisoer.Domain.Models;
using BellaFrisoer.Domain.Models.Discounts;
using System;

namespace BellaFrisoer.Application.Services
{
    public class BookingPriceService : IBookingPriceService
    {
        public decimal CalculateFinalPrice(
            Booking booking,
            Employee? employee,
            Treatment? treatment,
            Customer? customer,
            bool eventEnabled,
            DateTime? eventStartDate,
            DateTime? eventEndDate,
            TimeOnly? eventStartTime,
            TimeOnly? eventEndTime,
            decimal? eventPercent)
        {
            // Calculate base price
            var basePrice = booking.CalculateBasePrice();

            // Determine automatic tier discount
            var tierDiscount = GetTierDiscountForCustomer(customer);

            // Build event discount if applicable
            var (eventDiscount, eventAppliesNow) = BuildEventDiscountIfApplicable(
                eventEnabled,
                eventStartDate,
                eventEndDate,
                eventStartTime,
                eventEndTime,
                eventPercent,
                booking.BookingDate,
                booking.BookingStartTime);

            // Apply the best discount
            var priceWithTier = tierDiscount != null ? tierDiscount.Apply(basePrice) : basePrice;
            var priceWithEvent = (eventDiscount != null && eventAppliesNow) ? eventDiscount.Apply(basePrice) : basePrice;
            var finalPrice = Math.Min(priceWithTier, priceWithEvent);

            return finalPrice;
        }



        private IDiscountStrategy? GetTierDiscountForCustomer(Customer? customer)
        {
            if (customer is null) return null;

            int count = customer.Bookings?.Count ?? 0;

            if (count >= 20)
                return new GoldDiscount();
            if (count >= 10)
                return new SilverDiscount();
            if (count >= 5)
                return new BronzeDiscount();

            return null;
        }

        private (IDiscountStrategy? discount, bool appliesNow) BuildEventDiscountIfApplicable(
            bool eventEnabled,
            DateTime? eventStartDate,
            DateTime? eventEndDate,
            TimeOnly? eventStartTime,
            TimeOnly? eventEndTime,
            decimal? eventPercent,
            DateTime bookingDate,
            TimeOnly bookingStartTime)
        {
            if (!eventEnabled || eventPercent is null || eventPercent < 0m || eventPercent > 100m)
                return (null, false);

            if (eventStartDate is null || eventEndDate is null || eventStartTime is null || eventEndTime is null)
                return (null, false);

            var start = eventStartDate.Value.Date.Add(eventStartTime.Value.ToTimeSpan());
            var end = eventEndDate.Value.Date.Add(eventEndTime.Value.ToTimeSpan());

            if (end <= start)
                return (null, false);

            var bookingStart = bookingDate.Date.Add(bookingStartTime.ToTimeSpan());
            var appliesNow = bookingStart >= start && bookingStart <= end;

            var percentageDecimal = (eventPercent ?? 0m) / 100m;
            return (new EventDiscount(percentageDecimal), appliesNow);
        }
    }
}
