using BellaFrisoer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellaFrisoer.Domain.Interfaces
{
    public interface ICustomer
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public ICollection<Booking>? Bookings { get; set; }
        public long? PhoneNumber { get; set; }
    }
}
