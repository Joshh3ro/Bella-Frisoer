using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellaFrisoer.Domain.Models
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }
        public DateTime BookingDateTime { get; set; }
        public Customer Customer { get; set; }

        public Booking(DateTime bookingDateTime, Customer customer)
        {
            BookingDateTime = bookingDateTime;
            Customer = customer;
        }
    }
}
