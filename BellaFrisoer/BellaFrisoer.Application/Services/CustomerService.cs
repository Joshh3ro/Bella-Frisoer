using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BellaFrisoer.Application.Interfaces;
using BellaFrisoer.Domain.Interfaces;
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
            // Simple example: always allow creation.
            // Replace with real business logic if needed.
            return Task.FromResult(true);
        }

        public async Task AddCustomerAsync(Customer customer)
        {
            await _repository.AddAsync(customer);
        }

        public async Task<List<Customer>> GetAllAsync()
        {
            var customers = await _repository.GetAllAsync();
            return customers.ToList();
        }
    }
}