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

    public DateTime BookingStartTime { get; set; }

    public DateTime BookingEndTime { get; set; }

    // scalar FK: what EF uses & what forms bind to
    [Required]
    public int CustomerId { get; set; }

    [ForeignKey(nameof(CustomerId))]
    public Customer? Customer { get; set; }

    [Required]
    public int EmployeeId { get; set; }

    [ForeignKey(nameof(EmployeeId))]
    public Employee? Employee { get; set; }

    public Booking() { }

    public Booking(DateTime bookingDateTime, int customerId, int employeeId)
    {
        BookingDateTime = bookingDateTime;
        CustomerId = customerId;
        EmployeeId = employeeId;
    }

    public DateTime CombineDateTime(DateTime date, TimeSpan time)
    {
        return date.Date + time;
    }
}
