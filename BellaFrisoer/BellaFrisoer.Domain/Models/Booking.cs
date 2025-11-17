namespace BellaFrisoer.Domain.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Booking
{
    [Key]
    public int Id { get; set; }

    [Required]
    public DateTime BookingDateTime { get; set; }

    public int CustomerId { get; set; }

    [ForeignKey(nameof(CustomerId))]
    public Customer? Customer { get; set; }

    public Booking() { }

    // Make non-public so Blazor form mapping sees a single public ctor
    internal Booking(DateTime bookingDateTime, Customer customer)
    {
        BookingDateTime = bookingDateTime;
        Customer = customer;
        CustomerId = customer?.Id ?? 0;
    }
}
