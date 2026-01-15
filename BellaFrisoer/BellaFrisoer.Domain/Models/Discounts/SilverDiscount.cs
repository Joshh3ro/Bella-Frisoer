namespace BellaFrisoer.Domain.Models.Discounts;

public class SilverDiscount : IDiscountStrategy
{
    private const decimal Rate = 0.10m;
    private SilverDiscount(decimal rate)
    {
        rate = Rate;
    }

    public SilverDiscount Create()
    {
        return new SilverDiscount(Rate);
    }
    public decimal Apply(Booking booking, Customer customer)
    {
        return booking.BasePrice * Rate;
    }
}
