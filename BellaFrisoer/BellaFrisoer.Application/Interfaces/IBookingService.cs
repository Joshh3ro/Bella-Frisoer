using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BellaFrisoer.Domain.Models;

namespace BellaFrisoer.Application.Interfaces
{
    public interface IBookingService
    {
        Task<bool> CanCreateBookingAsync(Booking newBooking, CancellationToken cancellationToken = default);
        Task AddBookingAsync(Booking booking, CancellationToken cancellationToken = default);
        Task UpdateBookingAsync(Booking booking, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Booking>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Booking?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task DeleteBookingAsync(int id, CancellationToken cancellationToken = default);
        Task<List<Booking>> FilterBookingsAsync(string searchTerm);

    }
}