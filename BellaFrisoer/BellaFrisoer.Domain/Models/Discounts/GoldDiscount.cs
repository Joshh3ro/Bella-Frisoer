using BellaFrisoer.Domain.Interfaces;

namespace BellaFrisoer.Domain.Models.Discounts;

public class GoldDiscount : IDiscountStrategy
{
    private const decimal Rate = 0.15m;
    public decimal Apply(decimal amount)
    { return - (amount * Rate); }
}