namespace BellaFrisoer.Domain.Models.Discounts;

public class SilverDiscount : IDiscountStrategy
{
    private const decimal Rate = 0.10m;
    public SilverDiscount(){ }

    public decimal Apply(Booking booking, Customer customer)
    {
        return booking.BasePrice * Rate;
    }
}
