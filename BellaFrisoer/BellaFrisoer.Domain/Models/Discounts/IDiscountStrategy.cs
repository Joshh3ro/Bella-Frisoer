namespace BellaFrisoer.Domain.Models.Discounts;

public interface IDiscountStrategy
{
    // Opdatering til github
    decimal Apply(Booking booking, Customer customer);
}

