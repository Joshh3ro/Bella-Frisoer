namespace BellaFrisoer.Domain.Interfaces;

public interface IDiscountStrategy
{
    decimal Apply(decimal amount);
}