using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BellaFrisoer.Domain.Models;

namespace BellaFrisoer.Domain.Interfaces
{
    public interface ITreatmentRepository
    {
        Task<IReadOnlyList<Treatment>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Treatment?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task AddAsync(Treatment treatment, CancellationToken cancellationToken = default);
        Task UpdateAsync(Treatment treatment, CancellationToken cancellationToken = default);
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}