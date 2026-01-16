using BellaFrisoer.Domain.Models;
using BellaFrisoer.Domain.Models.Discounts;

namespace BellaFrisoer.Application.Interfaces;

public interface IDiscountCalculator
{
    // Kører alle strategier i CPU-bound tasks, deler RabatResult til opdatering.
    Task<DiscountResult> EvaluateAsync(Booking booking, Customer customer, IEnumerable<IDiscountStrategy> strategies, CancellationToken cancellationToken = default);
}