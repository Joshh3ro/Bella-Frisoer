using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BellaFrisoer.Domain.Models;

public class Customer
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public long? PhoneNumber { get; set; }

    public Customer(string name, long? phoneNumber = null)
    {
        Name = name;
        PhoneNumber = phoneNumber;
    }

    public Customer()
    {
    }
}