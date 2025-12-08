using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BellaFrisoer.Domain.Models;

namespace BellaFrisoer.Application.Interfaces
{
    // Small, focused repository interface. Returns IReadOnlyList for immutability,
    // and supports cancellation tokens for async operations.
    public interface ICustomerRepository
    {
        Task<IReadOnlyList<Customer>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Customer?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task AddAsync(Customer Customer, CancellationToken cancellationToken = default);
        // Add additional focused methods here instead of forcing consumers to retrieve everything.

        Task UpdateAsync(Customer customer, CancellationToken cancellationToken = default);

        Task DeleteAsync(int id, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<Customer>> FilterCustomersAsync(string searchTerm, CancellationToken cancellationToken = default);
    }
}