using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BellaFrisoer.Domain.Models
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime BookingDateTime { get; set; }

        public int CustomerId { get; set; }

        [ForeignKey(nameof(CustomerId))]
        public Customer Customer { get; set; }
        // public string Customer { get; set; }

        public Booking(DateTime bookingDateTime, Customer customer)
        {
            BookingDateTime = bookingDateTime;
            Customer = customer;
        }

        public Booking()
        {
            
        }
    }
}