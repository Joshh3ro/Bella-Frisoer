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
        Task<Employee> GetEmployeeByIdAsync(int id);
        Task DeleteEmployeeAsync(int id);

        Task<Employee?> UpdateEmployeeAsync(Employee employee);
        Task<List<Employee>> FilterEmployeesAsync(string searchTerm);
    }
}