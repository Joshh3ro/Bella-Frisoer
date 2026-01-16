using BellaFrisoer.Application.Interfaces;
using BellaFrisoer.Domain.Models;
using BellaFrisoer.Domain.Models.Discounts;

namespace BellaFrisoer.Application.Services;

public class LoyaltyDiscountStrategy : IDiscountCalculator
{
    public async Task<DiscountResult> EvaluateAsync(Booking booking, Customer customer, IEnumerable<IDiscountStrategy> strategies, CancellationToken cancellationToken = default)
    {
        Booking.ValidateBooking(booking);

        var result = new DiscountResult();

        // Hver strategi køres i en CPU-bound Task
        var tasks = strategies.Select(strategy => Task.Run(() =>
        {

            var discount = strategy.Apply(booking, customer);

            // Alle strategier forsøger at opdatere det delte RabatResult.
            // Metoden er dog locked så race-condition håndteres.
            var updated = result.TryUpdateIfBetter(discount, strategy);

        }, cancellationToken)).ToArray();

        // Vent på alle strategier
        await Task.WhenAll(tasks);

        return result;
    }
}