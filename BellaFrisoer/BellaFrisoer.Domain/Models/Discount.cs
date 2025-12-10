using BellaFrisoer.Domain.Interfaces;
namespace BellaFrisoer.Domain.Models;

public class Discount
{
    public IDiscountStrategy Strategy { get; set; }

    public Discount(IDiscountStrategy strategy)
    {
        Strategy = strategy;
    }

    public decimal Apply(decimal amount)
    {
        return Strategy.Apply(amount);
    }
}