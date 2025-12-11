using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BellaFrisoer.Application.Interfaces;
using BellaFrisoer.Domain.Models;

namespace BellaFrisoer.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repository;

        public CustomerService(ICustomerRepository repository)
        {
            _repository = repository;
        }

        public Task<bool> CanCreateCustomerAsync(Customer newCustomer)
        {
            return Task.FromResult(true);
        }

        public async Task AddCustomerAsync(Customer customer)
        {
            await _repository.AddAsync(customer);
        }

        public async Task DeleteCustomerAsync(Customer customer)
        {
            await _repository.DeleteAsync(customer.Id);
        }

        public async Task<List<Customer>> GetAllAsync()
        {
            var customers = await _repository.GetAllAsync();

            return customers.ToList();
        }
        public async Task<Customer?> GetCustomerByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }
        public async Task<Customer?> UpdateCustomerAsync(Customer customer)
        {
            await _repository.UpdateAsync(customer);
            return await _repository.GetByIdAsync(customer.Id);
        }

        public async Task<List<Customer>> FilterCustomersAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return await GetAllAsync();

        var filtered = await _repository.FilterCustomersAsync(searchTerm);
        return filtered.ToList();
        }

    }
}