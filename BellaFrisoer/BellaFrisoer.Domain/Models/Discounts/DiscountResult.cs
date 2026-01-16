namespace BellaFrisoer.Domain.Models.Discounts;

public sealed class DiscountResult
{
    private readonly object _sync = new();

    // Største fundne rabat (absolut beløb). 0 = ingen rabat endnu.
    public decimal BestDiscountAmount { get; private set; }

    // Hvilken strategi gav den bedste rabat.
    public IDiscountStrategy? BestStrategy { get; private set; }

    // Forsøg at opdatere hvis denne rabat er bedre (større) end den nuværende.
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