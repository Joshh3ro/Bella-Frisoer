namespace BellaFrisoer.Domain.Models.Discounts;

public interface IDiscountStrategy
{
    decimal Apply(decimal amount);
}
