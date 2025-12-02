using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BellaFrisoer.Domain.Models;

namespace BellaFrisoer.Domain.Interfaces
{
    // Small, focused repository interface. Returns IReadOnlyList for immutability,
    // and supports cancellation tokens for async operations.
    public interface IBookingRepository
    {
        Task<IReadOnlyList<Booking>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Booking>> GetByEmployeeIdAsync(int employeeId, CancellationToken cancellationToken = default);
        
        // New: get bookings for a specific employee on a specific date.
        // This reduces data transfer and helps the conflict-checker be efficient.
        Task<IReadOnlyList<Booking>> GetByEmployeeIdAndDateAsync(int employeeId, DateTime bookingDate, CancellationToken cancellationToken = default);

        Task<Booking?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task AddAsync(Booking booking, CancellationToken cancellationToken = default);
        // Add additional focused methods here instead of forcing consumers to retrieve everything.
    }
}