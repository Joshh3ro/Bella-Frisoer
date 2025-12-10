namespace BellaFrisoer.Domain.Models.Discounts;

public class EventDiscount : IDiscountStrategy
{
    private readonly decimal _percentage;

    public EventDiscount(decimal percentage)
    {
        _percentage = percentage;
    }

    public decimal Apply(decimal amount)
    { 
        return amount - (amount * _percentage); 
    }

    public bool IsBetterThan(IDiscountStrategy? currentDiscount)
    {
        if (currentDiscount == null ) return true;
        
        const decimal testAmount = 100;
        return Apply(testAmount) < currentDiscount.Apply(testAmount);
    }
}