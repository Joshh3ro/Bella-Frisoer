namespace BellaFrisoer.Domain.Models.Discounts;

public class GoldDiscount : IDiscountStrategy
{
    private const decimal Rate = 0.15m;
    public GoldDiscount()
    {
        
    }

    public decimal Apply(Booking booking, Customer customer)
    {
        return booking.BasePrice * Rate;
    }
}
