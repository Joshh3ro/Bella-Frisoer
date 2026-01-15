using BellaFrisoer.Domain.Models;
using BellaFrisoer.Domain.Queries;

namespace BellaFrisoer.Domain.Services
{

    /// <summary>
    /// Domain Service der checker booking konflikter
    /// </summary>
    public interface IBookingConflictChecker
    {

        /// <summary>
        /// Checks if a booking conflicts with existing bookings for the same employee
        /// </summary>
        Task<bool> HasConflictWithAny(Booking booking);

        /// <summary>
        /// Checks if a booking conflicts with existing bookings, excluding a specific booking (for updates)
        /// </summary>
        Task<bool> HasConflictWithUpdated(Booking booking, int excludeBookingId);
    }
}