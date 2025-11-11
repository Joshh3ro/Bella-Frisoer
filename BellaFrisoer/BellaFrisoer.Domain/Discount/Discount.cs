namespace BellaFrisoer.Domain.Discounts;

public class Discount
{
    private readonly IDiscountStrategy _strategy;

    public Discount(IDiscountStrategy strategy)
    {
        _strategy = strategy;
    }

    public decimal Apply(decimal amount)
    {
        return _strategy.Apply(amount);
    }
}
