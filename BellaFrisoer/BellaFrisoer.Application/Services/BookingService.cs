using System;
using System.Globalization;
using BellaFrisoer.Application.Interfaces;
using BellaFrisoer.Domain.Models;

//NOTE: Services bruges som vores logik til vores UI.

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

            var relevant = (await _repository.GetByEmployeeIdAndDateAsync(newBooking.EmployeeId, newBooking.BookingDate, cancellationToken)).ToList();
            return !_conflictChecker.HasBookingConflict(newBooking, relevant);
        }

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

        public async Task UpdateBookingAsync(Booking booking, CancellationToken cancellationToken = default)
        {
            if (booking is null) throw new ArgumentNullException(nameof(booking));
                
            // Optional: You could add conflict checking logic here for updates as well
                
            await _repository.UpdateAsync(booking, cancellationToken);
        }

        public async Task DeleteBookingAsync(Booking booking, CancellationToken cancellationToken = default)
        {
            await _repository.DeleteAsync(booking.Id, cancellationToken);
        }

        public async Task<IReadOnlyList<Booking>> GetAllAsync(CancellationToken cancellationToken = default)
            => await _repository.GetAllAsync(cancellationToken);

        public async Task<Booking?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
            => await _repository.GetByIdAsync(id, cancellationToken);
        public async Task<List<Booking>> FilterBookingsAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return (List<Booking>)await GetAllAsync();

            var filtered = await _repository.FilterBookingsAsync(searchTerm);
            return filtered.ToList();
        }
        public decimal CalculatePrice(Booking booking, Employee? employee, Treatment? treatment)
        {
            decimal computedPrice = 0m;

            if (treatment != null && treatment.Price > 0m)
                computedPrice = treatment.Price;

            if (employee != null && booking.BookingDuration > TimeSpan.Zero && employee.HourlyPrice > 0d)
            {
                var minutes = (decimal)booking.BookingDuration.TotalMinutes;
                var employeePart = ((decimal)employee.HourlyPrice / 60m) * minutes;
                computedPrice += employeePart;
            }

            return computedPrice;
        }

        public void UpdateDurationFromTreatment(Booking booking, Treatment? treatment)
        {
            if (booking == null)
                throw new ArgumentNullException(nameof(booking));

            if (treatment == null || treatment.Id <= 0)
            {
                booking.BookingDuration = TimeSpan.Zero;
                return;
            }

            booking.BookingDuration = TimeSpan.FromMinutes(treatment.Duration);
        }

        public (bool IsValid, string? ErrorMessage) ValidateBooking(Booking booking)
        {
            if (booking is null)
                return (false, "Booking mangler.");
            if (booking.CustomerId <= 0)
                return (false, "Vælg kunde...");
            if (booking.EmployeeId <= 0)
                return (false, "Vælg ansat...");
            if (booking.TreatmentId <= 0)
                return (false, "Vælg behandling...");
            if (booking.BookingStartTime == default)
                return (false, "Ugyldigt starttidspunkt.");
            if (booking.BookingDuration == default || booking.BookingDuration <= TimeSpan.Zero)
                return (false, "Ugyldig varighed.");
            return (true, null);
        }


    }
}