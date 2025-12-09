using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BellaFrisoer.Domain.Models;

public class Booking
{
    [Key]
    public int Id { get; set; }

    public DateTime BookingDateTime 
    {
        get { return CombineDateTime(BookingDate, BookingStartTime); } 
    }

    [Required]
    public DateTime BookingDate { get; set; }

    public TimeOnly BookingStartTime { get; set; }

    public TimeSpan BookingDuration { get; set; }

    public DateTime BookingEndTime
    {
        get 
        { 
            return CombineDateTime(BookingDate, BookingStartTime).Add(BookingDuration);
        }
    }

    // scalar FK: what EF uses & what forms bind to
    [Required]
    public int CustomerId { get; set; }

    public Customer? Customer { get; set; }

    [Required]
    public int EmployeeId { get; set; }

    [ForeignKey(nameof(EmployeeId))]
    public Employee? Employee { get; set; }

    public Booking()
    {
    }

    public DateTime CombineDateTime(DateTime date, TimeOnly time)
    {
        return date.Date.Add(time.ToTimeSpan());
    }
}
