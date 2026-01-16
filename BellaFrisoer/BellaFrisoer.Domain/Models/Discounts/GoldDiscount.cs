namespace BellaFrisoer.Domain.Models.Discounts;

public class GoldDiscount : IDiscountStrategy
{
    private const decimal Rate = 0.15m;
    public GoldDiscount()
    {
        
    }
    public decimal Apply(Booking booking, Customer customer)
    {
        if (customer.Bookings.Count >= 15)
        {
            return booking.BasePrice * Rate;
        }
        else return 0m;
    }
}




