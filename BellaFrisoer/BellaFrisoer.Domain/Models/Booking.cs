using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BellaFrisoer.Domain.Interfaces;

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
    public ICustomer? Customer { get; set; }

    [Required]
    public int EmployeeId { get; set; }

    [ForeignKey(nameof(EmployeeId))]
    public Employee? Employee { get; set; }

    public Booking() { }

    public Booking(DateTime bookingDateTime, int customerId, int employeeId)
    {
        BookingDateTime = bookingDateTime;
        CustomerId = customerId;
    }
}