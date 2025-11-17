namespace BellaFrisoer.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class Customer
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public string? Email { get; set; }
    public string? Address { get; set; }
    public long? PhoneNumber { get; set; }
    public DateTime? BirthDate { get; set; }

    public Customer() { }

    public Customer(string name, string? email = null, string? address = null, long? phoneNumber = null, DateTime? birthDate = null)
    {
        Name = name;
        Email = email;
        Address = address;
        PhoneNumber = phoneNumber;
        BirthDate = birthDate;
    }
}
