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
        /// Checker om en booking konflikter med eksisterende bookinger
        /// </summary>
        Task<bool> HasConflictWithAny(Booking booking);

        /// <summary>
        /// Checker om en opdateret booking konflikter med eksisterende bookinger, ekskluderer sig selv for  opdatering
        /// </summary>
        Task<bool> HasConflictWithUpdated(Booking booking, int excludeBookingId);
    }
}