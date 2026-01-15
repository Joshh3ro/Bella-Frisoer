using BellaFrisoer.Domain.Models;

namespace BellaFrisoer.Domain.Queries
{
    /// <summary>
    /// Repository interface for booking conflict checking (read-only, domain layer)
    /// </summary>
    public interface IBookingConflictQuery
    {
        /// <summary>
        /// Gets all bookings for a specific employee on a specific date
        /// </summary>
        Task<IEnumerable<Booking>> GetBookingsByEmployeeAndDateAsync(
            int employeeId, 
            DateTime date, 
            CancellationToken cancellationToken = default);
    }
}