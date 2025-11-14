using BellaFrisoer.Domain.Interfaces;
using BellaFrisoer.Domain.Models;

namespace BellaFrisoer.Application.Services
{
    public class BookingService
    {
        private readonly IBookingRepository _bookingRepository;

        public BookingService(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<IEnumerable<Booking>> GetAllBookingsAsync()
        {
            return await _bookingRepository.GetAllAsync();
        }

        public async Task<Booking?> GetBookingByIdAsync(int id)
        {
            return await _bookingRepository.GetByIdAsync(id);
        }

        public async Task AddBookingAsync(Booking booking)
        {
            // Example business rule: Prevent double booking
            var existing = await _bookingRepository.GetAllAsync();
            if (existing.Any(b => b.BookingDateTime == booking.BookingDateTime && b.CustomerId == booking.CustomerId))
                throw new InvalidOperationException("Customer already has a booking at this time.");

            await _bookingRepository.AddAsync(booking);
        }

        // Add other business logic methods as needed
    }
}