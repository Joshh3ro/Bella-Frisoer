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

            var tasks = strategies.Select(strategy => Task.Run(() =>
            {

                var discount = strategy.Apply(booking, customer);

                // Alle vores strategier forsøger at opdatere RabatResult.
                // men metoden er locked så race-condition håndteres.
                var updated = result.TryUpdateIfBetter(discount);

            }, cancellationToken)).ToArray();

            await Task.WhenAll(tasks);

            return result;
        }
    }