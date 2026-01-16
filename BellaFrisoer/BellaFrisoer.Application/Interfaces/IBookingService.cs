using BellaFrisoer.Application.DTOs;
using BellaFrisoer.Domain.Models;
using BellaFrisoer.Domain.Models.Discounts;

namespace BellaFrisoer.Application.Interfaces
{
    public interface IBookingService
    {
        Task AddBookingAsync(BookingCreateDto dto, CancellationToken cancellationToken = default);
        Task UpdateBookingAsync(BookingUpdateDto dto, CancellationToken cancellationToken = default);
        Task DeleteBookingAsync(Booking booking, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Booking>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Booking?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<Booking>> FilterBookingsAsync(string searchTerm, CancellationToken cancellationToken = default);
        IDiscountStrategy? GetDiscountStrategyForCustomerTotalBookings(Customer customer);
    }
}
