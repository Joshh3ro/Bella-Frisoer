<<<<<<< HEAD
﻿namespace BellaFrisoer.Domain.Models;
using System.ComponentModel.DataAnnotations;
=======
﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BellaFrisoer.Domain.Interfaces;
using BellaFrisoer.Domain.Models;
>>>>>>> Oskar

public class Customer : IPerson
{
    [Key]
<<<<<<< HEAD
    public int Id { get; set; }
    [Required]
    public string? Name { get; set; }
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>(); public string? Email { get; set; }
    public string? Address { get; set; }
    public long? PhoneNumber { get; set; }
    public DateTime? BirthDate { get; set; }

    public Customer( string name, string? email, string? address, int? phoneNumber, DateTime? birthDate )
    {
        Name = name;
        Email = email;
        Address = address;
        PhoneNumber = phoneNumber;
        BirthDate = birthDate;
    }

    public Customer()
    {
        
    }
=======
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public ICollection<Booking>? Bookings { get; set; } = new List<Booking>();
    public long? PhoneNumber { get; set; }

    public Customer() { }
>>>>>>> Oskar
}