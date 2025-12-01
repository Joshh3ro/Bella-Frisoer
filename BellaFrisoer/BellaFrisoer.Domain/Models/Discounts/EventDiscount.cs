using BellaFrisoer.Domain.Interfaces;

namespace BellaFrisoer.Domain.Models.Discounts;

public class EventDiscount : IDiscountStrategy
{
    private readonly decimal _percentage;

    public EventDiscount(decimal percentage)
    {
        _percentage = percentage;
    }
    
    public decimal Apply(decimal amount)
    { return amount - (amount * _percentage); }
    
    //TODO: Compare current discounts applied to the user to see if they qualify for this discount and if their discount is better or worse if better then dont apply

    public bool IsBetterThan(IDiscountStrategy? currentDiscount)
    {
        if (currentDiscount == null ) return true;
        const decimal testAmount = 100;
        return Apply(testAmount) < currentDiscount.Apply(testAmount);
    }
}