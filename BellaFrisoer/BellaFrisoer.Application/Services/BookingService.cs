using BellaFrisoer.Application.Interfaces;
using BellaFrisoer.Domain.Models;
using BellaFrisoer.Infrastructure;
using BellaFrisoer.Domain.Interfaces;

namespace BellaFrisoer.Application.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _repository;
        private readonly IBookingConflictChecker _conflictChecker;

        public BookingService(IBookingRepository repository, IBookingConflictChecker conflictChecker)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _conflictChecker = conflictChecker ?? throw new ArgumentNullException(nameof(conflictChecker));
        }

        public async Task<bool> CanCreateBookingAsync(Booking newBooking, CancellationToken cancellationToken = default)
        {
            if (newBooking is null) throw new ArgumentNullException(nameof(newBooking));
            if (newBooking.BookingDuration <= TimeSpan.Zero) return false; // validate early

            // More efficient: only fetch bookings for that employee on the booking date
            var relevant = (await _repository.GetByEmployeeIdAndDateAsync(newBooking.EmployeeId, newBooking.BookingDate, cancellationToken)).ToList();
            return !_conflictChecker.HasBookingConflict(newBooking, relevant);
        }

        // Improved: check for conflicts before adding and throw a clear exception
        public async Task AddBookingAsync(Booking booking, CancellationToken cancellationToken = default)
        {
            if (booking is null) throw new ArgumentNullException(nameof(booking));
            if (booking.BookingDuration <= TimeSpan.Zero) throw new ArgumentException("Booking duration must be positive.", nameof(booking));

            if (!await CanCreateBookingAsync(booking, cancellationToken))
            {
                throw new InvalidOperationException("Cannot add booking: time conflict with existing booking.");
            }

            await _repository.AddAsync(booking, cancellationToken);
        }

        public async Task<IReadOnlyList<Booking>> GetAllAsync(CancellationToken cancellationToken = default)
            => await _repository.GetAllAsync(cancellationToken);

        public async Task<Booking?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
            => await _repository.GetByIdAsync(id, cancellationToken);
    }
}