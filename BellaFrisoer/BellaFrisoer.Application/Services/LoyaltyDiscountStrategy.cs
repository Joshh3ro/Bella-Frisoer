using BellaFrisoer.Application.Interfaces;
using BellaFrisoer.Domain.Models;
using BellaFrisoer.Domain.Models.Discounts;
using BellaFrisoer.Domain.Services;


namespace BellaFrisoer.Application.Services;

public class LoyaltyDiscountStrategy : IDiscountCalculator
{
    private readonly IBookingConflictChecker _bookingConflictChecker;

    public LoyaltyDiscountStrategy(IBookingConflictChecker bookingConflictChecker)
    {
        _bookingConflictChecker = bookingConflictChecker;
    }
    public async Task<DiscountResult> EvaluateAsync(Booking booking, Customer customer, IEnumerable<IDiscountStrategy> strategies, CancellationToken cancellationToken = default)
    {
        booking.ValidateBooking();

        var result = new DiscountResult();

        var tasks = strategies.Select(strategy => Task.Run(() =>
        {

            var discount = strategy.Apply(booking, customer);

            // Alle vores strategier forsøger at opdatere RabatResult.
            // men metoden er locked så race-conditions håndteres.
            result.UpdateIfBetter(discount);

        }, cancellationToken)).ToArray();

        await Task.WhenAll(tasks);

        return result;
    }
}