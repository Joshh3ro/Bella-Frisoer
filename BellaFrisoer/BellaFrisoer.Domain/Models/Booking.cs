using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellaFrisoer.Domain.Models
{
    public class Booking
    {
        public DateTime BookingDateTime { get; set; }
        public string Customer { get; set; }

        public Booking(DateTime bookingDateTime, string customer)
        {
            BookingDateTime = bookingDateTime;
            Customer = customer;

        }
    }
}
