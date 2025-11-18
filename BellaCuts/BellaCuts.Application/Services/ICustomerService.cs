using BellaCuts.Domain.Entities;

namespace BellaCuts.Application.Services;

public interface ICustomerService
{
    Task<IEnumerable<Customer>> GetAllAsync(CancellationToken cancellation = default);
    Task<Customer?> GetAsync(Guid id, CancellationToken cancellation = default);
    Task AddAsync(Customer customer, CancellationToken cancellation = default);
    Task UpdateAsync(Customer customer, CancellationToken cancellation = default);
    Task DeleteAsync(Guid id, CancellationToken cancellation = default);
}