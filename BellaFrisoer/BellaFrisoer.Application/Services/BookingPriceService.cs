using BellaFrisoer.Application.Interfaces;
using BellaFrisoer.Application.DTOs;
using BellaFrisoer.Domain.Models;
using BellaFrisoer.Domain.Models.Discounts;

namespace BellaFrisoer.Application.Services;

public class BookingPriceService : IBookingPriceService
{
    private readonly IDiscountCalculator _discountCalculator;

    public BookingPriceService(IDiscountCalculator discountCalculator)
    {
        _discountCalculator = discountCalculator;
    }

    public async Task<decimal> CalculateFinalPrice(UpdatePriceDto dto, Customer customer, Employee employee, Treatment treatment)
    {
        // Opretter en midlertidig booking til prisberegning
        var booking = Booking.Create(
            customer, 
            employee, 
            treatment, 
            dto.BookingDate, 
            dto.BookingStartTime);

        var basePrice = booking.CalculateBasePrice();

        // Opretter strategier på en liste, som senere kan køres som tasks.
        var strategies = new List<IDiscountStrategy>
        {
            new BronzeDiscount(),
            new SilverDiscount(),
            new GoldDiscount(),
        };
        // Afventer bedste rabatresultat fra LoyaltyDiscountStrategy
        var discountResult = await _discountCalculator.EvaluateAsync(
            booking,
            customer,
            strategies
        );
        // Beregner den komplette pris, og anvender automatisk den bedste rabat.
        var finalPrice = basePrice - discountResult.BestDiscountAmount;

        return finalPrice;
    }
}
