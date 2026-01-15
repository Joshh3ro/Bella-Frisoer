namespace BellaFrisoer.Domain.Models.Discounts;

public class EventDiscount : IDiscountStrategy
{
    private readonly decimal _percentage;

    public EventDiscount(decimal percentage)
    {
        _percentage = percentage;
    }

    // Returnerer rabatbeløbet (ikke final price) => konsistent med IDiscountStrategy
    public decimal Apply(Booking booking, Customer customer)
    {
        return booking.BasePrice * _percentage;
    }
}