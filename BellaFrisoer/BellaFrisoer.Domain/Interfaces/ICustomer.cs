using BellaFrisoer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellaFrisoer.Domain.Interfaces
{
    public interface ICustomer : IPerson
    {
        public ICollection<Booking>? Bookings { get; set; }
        public string Note { get; set; }
    }
}
