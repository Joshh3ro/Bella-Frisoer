using System.Collections.Generic;
using System.Threading.Tasks;
using BellaFrisoer.Domain.Models;

namespace BellaFrisoer.Application.Interfaces
{
    public interface IEmployeeService
    {
        Task<bool> CanCreateEmployeeAsync(Employee newEmployee);
        Task AddEmployeeAsync(Employee employee);
        Task<List<Employee>> GetAllAsync();
    }
}