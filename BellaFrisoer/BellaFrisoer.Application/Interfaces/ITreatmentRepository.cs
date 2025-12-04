using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BellaFrisoer.Domain.Models;

namespace BellaFrisoer.Application.Interfaces
{
    // Small, focused repository interface. Returns IReadOnlyList for immutability,
    // and supports cancellation tokens for async operations.
    public interface ITreatmentRepository
    {
        Task<IReadOnlyList<Treatment>> GetAllAsync(CancellationToken cancellationToken = default);
       
        Task<Treatment?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task AddAsync(Treatment Treatment, CancellationToken cancellationToken = default);
        // Add additional focused methods here instead of forcing consumers to retrieve everything.

        Task UpdateAsync(Treatment treatment, CancellationToken cancellationToken = default);
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);

    }
}