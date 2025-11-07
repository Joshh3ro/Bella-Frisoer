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
        public string CustomerName { get; set; }

        public Booking(DateTime bookingDateTime, string customerName)
        {
            BookingDateTime = bookingDateTime;
            CustomerName = customerName;

        }
    }
}
