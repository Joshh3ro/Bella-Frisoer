namespace BellaFrisoer.Domain.Models.Discounts;

public class GoldDiscount : IDiscountStrategy
{
    private const decimal Rate = 0.15m;
    private GoldDiscount(decimal rate)
    {
        rate = Rate;
    }

    public GoldDiscount Create()
    {
        return new GoldDiscount(Rate);
    }
    public decimal Apply(Booking booking, Customer customer)
    {
        return booking.BasePrice * Rate;
    }
}
