using System.Collections.Generic;
using System.Threading.Tasks;
using BellaFrisoer.Domain.Models;

namespace BellaFrisoer.Application.Interfaces
{
    public interface ICustomerService
    {
        Task<bool> CanCreateCustomerAsync(Customer newCustomer);
        Task AddCustomerAsync(Customer customer);
        Task DeleteCustomerAsync(Customer customer);
        Task<List<Customer>> GetAllAsync();
        Task<Customer> GetCustomerByIdAsync(int id);
        Task<Customer> UpdateCustomerAsync(Customer customer);
    }
}