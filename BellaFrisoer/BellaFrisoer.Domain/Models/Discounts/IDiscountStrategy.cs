namespace BellaFrisoer.Domain.Models.Discounts;

public interface IDiscountStrategy
{
    decimal Apply(Booking booking, Customer customer);
}
