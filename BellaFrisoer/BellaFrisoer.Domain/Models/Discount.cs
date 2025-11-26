using BellaFrisoer.Domain.Interfaces;

namespace BellaFrisoer.Domain.Models;

public class Discount
{
    private IDiscountStrategy _strategy;
    public Discount(IDiscountStrategy strategy)
    { _strategy = strategy; }
    public void SetStrategy(IDiscountStrategy strategy)
    { _strategy = strategy; }
    public decimal Apply(decimal amount)
    { return _strategy.Apply(amount); }
}