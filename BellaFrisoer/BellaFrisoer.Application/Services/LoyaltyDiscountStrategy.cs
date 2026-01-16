    using BellaFrisoer.Application.Interfaces;
    using BellaFrisoer.Domain.Models;
    using BellaFrisoer.Domain.Models.Discounts;

    namespace BellaFrisoer.Application.Services;

    public class LoyaltyDiscountStrategy : IDiscountCalculator
    {
        public async Task<DiscountResult> EvaluateAsync(Booking booking, Customer customer, IEnumerable<IDiscountStrategy> strategies, CancellationToken cancellationToken = default)
        {
            booking.ValidateBooking(booking);

            var result = new DiscountResult();

            var tasks = strategies.Select(strategy => Task.Run(() =>
            {

                var discount = strategy.Apply(booking, customer);

                // Alle vores strategier fors�ger at opdatere RabatResult.
                // men metoden er locked s� race-condition h�ndteres.
                var updated = result.TryUpdateIfBetter(discount, strategy);

            }, cancellationToken)).ToArray();

            await Task.WhenAll(tasks);

            return result;
        }
    }