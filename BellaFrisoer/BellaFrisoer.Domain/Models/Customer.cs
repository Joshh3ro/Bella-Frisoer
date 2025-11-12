namespace BellaFrisoer.Domain.Models;
using System.ComponentModel.DataAnnotations;

public class Customer
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>(); public string? Email { get; set; }
    public string? Address { get; set; }
    public long? PhoneNumber { get; set; }
    public DateTime? BirthDate { get; set; }

    public Customer( string? Name, string? Email, string? Address, int? PhoneNumber, DateTime? BirthDate )
    {
        Name = Name;
        Email = Email;
        Address = Address;
        PhoneNumber = PhoneNumber;
        BirthDate = BirthDate;

    }
    public Customer()
    {

    }
}