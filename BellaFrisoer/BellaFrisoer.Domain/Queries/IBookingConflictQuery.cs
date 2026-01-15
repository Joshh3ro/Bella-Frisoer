using BellaFrisoer.Domain.Models;

namespace BellaFrisoer.Domain.Queries
{
    /// <summary>
    /// Query der henter potentielle booking konflikter for bookingconflictchecker (read-only, domain layer)
    /// </summary>
    public interface IBookingConflictQuery
    {
        /// <summary>
        /// Henter alle bookings for en given medarbejder på en given dato
        /// </summary>
        Task<IEnumerable<Booking>> GetBookingsByEmployeeAndDateAsync(
            int employeeId, 
            DateTime date, 
            CancellationToken cancellationToken = default);
    }
}