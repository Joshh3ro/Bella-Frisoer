namespace BellaFrisoer.Domain.Models.Discounts;

public class BronzeDiscount : IDiscountStrategy
{
    private const decimal Rate = 0.05m;

    public BronzeDiscount(){}

    public decimal Apply(Booking booking, Customer customer)
    {
        if (customer.Bookings.Count >= 5)
        {
            return booking.BasePrice * Rate;
        }
        else return 0m;
    }
}