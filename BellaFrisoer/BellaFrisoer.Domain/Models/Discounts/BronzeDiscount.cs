namespace BellaFrisoer.Domain.Models.Discounts;

public class BronzeDiscount : IDiscountStrategy
{
    private const decimal Rate = 0.05m;
    
    public decimal Apply(decimal amount)
    { 
        return amount - (amount * Rate); 
    }
}