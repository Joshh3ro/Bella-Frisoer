namespace BellaFrisoer.Domain.Models.Discounts;

// Corrected GoldDiscount class structure
public class GoldDiscount : IDiscountStrategy
{
    private const decimal Rate = 0.15m;
    
    public decimal Apply(decimal amount)
    { 
        return amount - (amount * Rate); 
    }
}