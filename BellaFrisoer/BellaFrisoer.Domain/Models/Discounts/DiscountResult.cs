namespace BellaFrisoer.Domain.Models.Discounts;

public sealed class DiscountResult
{
    private readonly object _sync = new();
    public decimal BestDiscountAmount { get; private set; }
    public IDiscountStrategy? BestStrategy { get; private set; }
    public bool TryUpdateIfBetter(decimal discountAmount, IDiscountStrategy strategy)
    {
        lock (_sync)
        {
            if (discountAmount > BestDiscountAmount)
            {
                BestDiscountAmount = discountAmount;
                BestStrategy = strategy;
                return true;
            }
            return false;
        }
    }
}