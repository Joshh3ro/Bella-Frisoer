using BellaFrisoer.Application.Interfaces;
using BellaFrisoer.Domain.Models;
using BellaFrisoer.Domain.Models.Discounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BellaFrisoer.Application.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _repository;
        private readonly IBookingConflictChecker _conflictChecker;
        private readonly IBookingPriceService _bookingPriceService;

        public BookingService(
            IBookingRepository repository,
            IBookingConflictChecker conflictChecker,
            IBookingPriceService bookingPriceService)
        {
            _repository = repository;
            _conflictChecker = conflictChecker;
            _bookingPriceService = bookingPriceService;
        }

        public async Task<bool> CanCreateBookingAsync(Booking newBooking, CancellationToken cancellationToken = default)
        {
            if (newBooking is null) throw new ArgumentNullException(nameof(newBooking));
            if (newBooking.BookingDuration <= TimeSpan.Zero) return false;

            var relevant = await _repository.GetByEmployeeIdAndDateAsync(newBooking.Employee.Id, newBooking.BookingDate, cancellationToken);
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

        public async Task<List<Booking>> FilterBookingsAsync(string searchTerm, CancellationToken cancellationToken = default)
            => await _repository.FilterBookingsAsync(searchTerm, cancellationToken);
        public bool HasBookingConflict(Booking newBooking, IEnumerable<Booking> existingBookings)
        {
            // .Any looper over hele listen og sammenligner.
            return existingBookings.Any(b => newBooking.ConflictsWith(b));
        }

        public IDiscountStrategy? GetDiscountStrategyForCustomerTotalBookings(Customer customer)
        {
            if (customer is null) return null;

            int count = customer.Bookings?.Count ?? 0;

            if (count >= 20)
                return new GoldDiscount();
            if (count >= 10)
                return new SilverDiscount();
            if (count >= 5)
                return new BronzeDiscount();

            return null;
        }


        public decimal CalculatePrice(Booking booking, Employee? employee, Treatment? treatment, Customer? customer = null)
        {
            return _bookingPriceService.CalculateFinalPrice(
                booking,
                employee,
                treatment,
                customer,
                false,
                null,
                null,
                null,
                null,
                null);
        }


    }
}
