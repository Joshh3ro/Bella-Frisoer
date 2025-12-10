using BellaFrisoer.Domain.Interfaces;

namespace BellaFrisoer.Domain.Models.Discounts;

public class SilverDiscount : IDiscountStrategy
{
    private const decimal Rate = 0.10m;
    public decimal Apply(decimal amount)
    { return amount - (amount * Rate); }
}