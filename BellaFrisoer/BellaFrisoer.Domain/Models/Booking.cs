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
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerAddress { get; set; }
        public long? CustomerPhoneNumber { get; set; }
        public DateTime? CustomerBirthDate { get; set; }

        public Booking(DateTime bookingDateTime, string customerName, string customerEmail, string customerAddress, int customerPhoneNumber, DateTime? customerBirthDate )
        {
            BookingDateTime = bookingDateTime;
            CustomerName = customerName;
            CustomerEmail = customerEmail;
            CustomerAddress = customerAddress;
            CustomerPhoneNumber = customerPhoneNumber;
            CustomerBirthDate = customerBirthDate;

        }
    }
}
