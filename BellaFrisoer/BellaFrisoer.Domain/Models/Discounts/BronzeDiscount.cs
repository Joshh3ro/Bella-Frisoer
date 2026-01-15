namespace BellaFrisoer.Domain.Models.Discounts;

public class BronzeDiscount : IDiscountStrategy
{
    private const decimal Rate = 0.05m;



    public BronzeDiscount(){}


    public decimal Apply(Booking booking, Customer customer)
    { 
        return booking.BasePrice * Rate; 
    }


}