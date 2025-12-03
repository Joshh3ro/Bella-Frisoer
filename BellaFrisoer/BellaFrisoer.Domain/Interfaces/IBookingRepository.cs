using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BellaFrisoer.Domain.Interfaces;

namespace BellaFrisoer.Domain.Interfaces
{
    // Small, focused repository interface. Returns IReadOnlyList for immutability,
    // and supports cancellation tokens for async operations.
    public interface IBookingRepository
    {
        Task<IReadOnlyList<IBooking>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<IReadOnlyList<IBooking>> GetByEmployeeIdAsync(int employeeId, CancellationToken cancellationToken = default);
        
        // New: get bookings for a specific employee on a specific date.
        // This reduces data transfer and helps the conflict-checker be efficient.
        Task<IReadOnlyList<IBooking>> GetByEmployeeIdAndDateAsync(int employeeId, DateTime bookingDate, CancellationToken cancellationToken = default);

        Task<IBooking?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task AddAsync(IBooking booking, CancellationToken cancellationToken = default);
        // Add additional focused methods here instead of forcing consumers to retrieve everything.
    }
}