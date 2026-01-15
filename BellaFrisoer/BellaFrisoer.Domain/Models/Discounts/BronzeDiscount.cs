namespace BellaFrisoer.Domain.Models.Discounts;

public class BronzeDiscount : IDiscountStrategy
{
    private const decimal Rate = 0.5m;



    private BronzeDiscount()
    {

    }

    public BronzeDiscount Create()
    {
        return new BronzeDiscount();
    }

    public decimal Apply(Booking booking, Customer customer)
    { 
        return booking.BasePrice * Rate; 
    }


}