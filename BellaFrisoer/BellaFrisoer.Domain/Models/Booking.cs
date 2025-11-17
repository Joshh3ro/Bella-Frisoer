using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BellaFrisoer.Domain.Models;

public class Booking
{
    [Key]
    public int Id { get; set; }

    [Required]
    public DateTime BookingDateTime { get; set; }

    // scalar FK: what EF uses & what forms bind to
    [Required]
    public int CustomerId { get; set; }

    [ForeignKey(nameof(CustomerId))]
    public Customer? Customer { get; set; }

    public Booking() { }

    public Booking(DateTime bookingDateTime, int customerId)
    {
        BookingDateTime = bookingDateTime;
        CustomerId = customerId;
    }
}
