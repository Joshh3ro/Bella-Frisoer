namespace BellaFrisoer.Domain.Models.Discounts;

public sealed class DiscountResult
{
    private readonly object _sync = new();
    public decimal BestDiscountAmount { get; private set; }
    public void UpdateIfBetter(decimal discountAmount)
    {
        lock (_sync)
        {
            if (discountAmount > BestDiscountAmount && discountAmount > 0)
            {
                BestDiscountAmount = discountAmount;
            }
        }
    }
}




