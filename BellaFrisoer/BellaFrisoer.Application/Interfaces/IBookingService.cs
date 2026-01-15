using BellaFrisoer.Application.Contracts.Commands;
using BellaFrisoer.Domain.Models;
using BellaFrisoer.Domain.Models.Discounts;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BellaFrisoer.Application.Interfaces
{
    public interface IBookingService
    {
        Task AddBookingAsync(BookingCreateDto dto, CancellationToken cancellationToken = default);
        Task UpdateBookingAsync(Booking booking, CancellationToken cancellationToken = default);
        Task DeleteBookingAsync(Booking booking, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Booking>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Booking?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<Booking>> FilterBookingsAsync(string searchTerm, CancellationToken cancellationToken = default);
        IDiscountStrategy? GetDiscountStrategyForCustomerTotalBookings(Customer customer);


    }
}
