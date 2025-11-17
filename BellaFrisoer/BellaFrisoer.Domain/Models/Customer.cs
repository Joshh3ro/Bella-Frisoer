using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BellaFrisoer.Domain.Models;

public class Customer
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;


    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public long? PhoneNumber { get; set; }


    public Customer() { }
}