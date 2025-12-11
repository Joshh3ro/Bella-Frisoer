using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BellaFrisoer.Domain.Models;

namespace BellaFrisoer.Application.Interfaces
{
    public interface ITreatmentRepository
    {
        Task<IReadOnlyList<Treatment>> GetAllAsync(CancellationToken cancellationToken = default);
       
        Task<Treatment?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task AddAsync(Treatment Treatment, CancellationToken cancellationToken = default);

        Task UpdateAsync(Treatment treatment, CancellationToken cancellationToken = default);
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<List<Treatment>> FilterTreatmentsAsync(string searchTerm, CancellationToken cancellationToken = default);

    }
}