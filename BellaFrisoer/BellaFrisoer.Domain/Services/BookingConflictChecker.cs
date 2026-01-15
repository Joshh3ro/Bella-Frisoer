using BellaFrisoer.Domain.Models;
using BellaFrisoer.Domain.Queries;

namespace BellaFrisoer.Domain.Services
{
    /// <summary>
    /// Service der checker booking conflict
    /// </summary>
    public class BookingConflictChecker : IBookingConflictChecker
    {
        private readonly IBookingConflictQuery _conflictQuery;

        public BookingConflictChecker(IBookingConflictQuery conflictQuery)
        {
            _conflictQuery = conflictQuery ?? throw new ArgumentNullException(nameof(conflictQuery));
        }

        /// <summary>
        /// Checker om en booking har konflikt med eksisterende bookings
        /// </summary>
        public async Task<bool> HasConflictWithAny(Booking booking)
        {
            var existingBookings = await _conflictQuery
                .GetBookingsByEmployeeAndDateAsync(booking.Employee.Id, booking.BookingDate);

            return existingBookings.Any(existing => booking.ConflictsWith(existing));
        }

        public async Task<bool> HasConflictWithUpdated(Booking booking, int excludeBookingId)
        {
            var existingBookings = await _conflictQuery
                .GetBookingsByEmployeeAndDateAsync(booking.Employee.Id, booking.BookingDate);

            var filteredBookings = FilterExcludedBooking(existingBookings, excludeBookingId);
            return filteredBookings.Any(existing => booking.ConflictsWith(existing));
        }

        /// <summary>
        /// Filtrerer en specifik booking ud fra samlingen (bruges til opdateringsscenarier)
        /// </summary>
        private static IEnumerable<Booking> FilterExcludedBooking(IEnumerable<Booking> bookings, int excludeBookingId)
        {
            return bookings.Where(b => b.Id != excludeBookingId);
        }
    }
}