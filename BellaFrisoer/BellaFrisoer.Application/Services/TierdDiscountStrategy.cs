using BellaFrisoer.Domain.Models.Discounts;

namespace BellaFrisoer.Application.Services;

public class TieredDiscountStrategy : IDiscountStrategy

{
    private readonly Dictionary<decimal, IDiscountStrategy> _tiers;

    public TieredDiscountStrategy(Dictionary<decimal, IDiscountStrategy> tiers)
    {
        _tiers = tiers;
    }

    public decimal Apply(decimal amount)
    {
        // Find the applicable tier: highest threshold less than or equal to amount
        var applicableTier = _tiers
            .Where(tier => amount >= tier.Key)
            .OrderByDescending(tier => tier.Key)
            .FirstOrDefault();

        // If a matching strategy is found, use it; otherwise, return original amount
        var strategy = applicableTier.Value;
        
        return strategy != null ? strategy.Apply(amount) : amount;
    }
}