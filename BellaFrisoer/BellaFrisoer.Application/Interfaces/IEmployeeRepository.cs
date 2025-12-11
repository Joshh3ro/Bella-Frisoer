using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BellaFrisoer.Domain.Models;

namespace BellaFrisoer.Application.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<IReadOnlyList<Employee>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Employee?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task AddAsync(Employee Employee, CancellationToken cancellationToken = default);

        Task UpdateAsync(Employee employee, CancellationToken cancellationToken = default);

        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<List<Employee>> FilterEmployeesAsync(string searchTerm, CancellationToken cancellationToken = default);

    }
}