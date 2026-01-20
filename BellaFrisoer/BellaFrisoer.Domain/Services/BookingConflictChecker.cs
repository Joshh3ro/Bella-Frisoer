using BellaFrisoer.Domain.Models;
using BellaFrisoer.Domain.Queries;

namespace BellaFrisoer.Domain.Services
{
    /// <summary>
    /// Domain service der checker booking conflicts.
    /// </summary>
    public class BookingConflictChecker : IBookingConflictChecker
    {
        private readonly IBookingConflictQuery _conflictQuery;

        public BookingConflictChecker(IBookingConflictQuery conflictQuery)
        {
            _conflictQuery = conflictQuery ?? throw new ArgumentNullException(nameof(conflictQuery));
        }

        /// <summary>
        /// Checker om en booking har konflikt med eksisterende bookings.
        /// </summary>
        public async Task<bool> HasConflictWithAny(Booking booking)
        {
            var existingBookings = await _conflictQuery
                .GetBookingsByEmployeeAndDateAsync(booking.Employee.Id, booking.BookingDate);

            return existingBookings.Any(existing => Conflicts(booking, existing));
        }

        /// <summary>
        /// Checker om booking har konflikt med eksisterende bookings,
        /// men ekskluderer en specifik booking (bruges ved update).
        /// </summary>
        public async Task<bool> HasConflictWithUpdated(Booking booking, int excludeBookingId)
        {
            var existingBookings = await _conflictQuery
                .GetBookingsByEmployeeAndDateAsync(booking.Employee.Id, booking.BookingDate);

            var filtered = existingBookings.Where(b => b.Id != excludeBookingId);

            return filtered.Any(existing => Conflicts(booking, existing));
        }


        private static bool Conflicts(Booking a, Booking b)
        {
            if (a == null || b == null)
                throw new ArgumentNullException();

            if (a.Employee.Id != b.Employee.Id)
                return false;

            if (a.BookingDuration <= TimeSpan.Zero || b.BookingDuration <= TimeSpan.Zero)
                return false;

            var aStart = Combine(a.BookingDate, a.BookingStartTime);
            var aEnd = aStart.Add(a.BookingDuration);

            var bStart = Combine(b.BookingDate, b.BookingStartTime);
            var bEnd = bStart.Add(b.BookingDuration);

            return aStart < bEnd && bStart < aEnd;
        }

        private static DateTime Combine(DateTime date, TimeOnly time)
        {
            return date.Date.Add(time.ToTimeSpan());
        }
    }
}
