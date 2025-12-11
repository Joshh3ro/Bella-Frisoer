using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellaFrisoer.Application.Interfaces
{
    public interface IBookingConflictChecker
    {
        public bool HasBookingConflict(Domain.Models.Booking newBooking, IEnumerable<Domain.Models.Booking> existingBookings);
    }
}
