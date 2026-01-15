using BellaFrisoer.Domain.Models.Discounts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BellaFrisoer.Domain.Models;

public class Customer
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; }
    public long PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Note { get; set; }
    public ICollection<Booking>? Bookings { get; set; } = new List<Booking>();


    public Customer() { }

    public IDiscountStrategy? GetLoyaltyDiscount(Customer customer)
    {
        if (customer is null) return null;

        int count = customer.Bookings?.Count ?? 0;

        if (count >= 20)
            return new GoldDiscount();
        if (count >= 10)
            return new SilverDiscount();
        if (count >= 5)
            return new BronzeDiscount();

        return null;
    }
}