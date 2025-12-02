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
        int Id { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        long PhoneNumber { get; set; }
        string? Email { get; set; }
        string? Note { get; set; }
        ICollection<Booking>? Bookings { get; set; }
    }
}
