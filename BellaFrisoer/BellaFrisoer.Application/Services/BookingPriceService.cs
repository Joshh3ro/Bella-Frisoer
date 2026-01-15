using BellaFrisoer.Application.Interfaces;
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

    public async Task<decimal> CalculateFinalPrice(Booking booking, Customer customer)
    {
        if (booking is null) throw new ArgumentNullException(nameof(booking));
        if (customer is null) throw new ArgumentNullException(nameof(customer));

        // 1. Base price fra domænet
        var basePrice = booking.CalculateBasePrice();

        // 2. Kør parallel rabatberegning
        var strategies = new List<IDiscountStrategy>
        {
            new BronzeDiscount(),
            new SilverDiscount(),
            new GoldDiscount(),
        };

        var rabatResult = await _discountCalculator.EvaluateAsync(
            booking,
            customer,
            strategies
        );

        // 3. Træk bedste rabat fra base price
        var finalPrice = basePrice - rabatResult.BestDiscountAmount;

        return finalPrice;
    }
}
