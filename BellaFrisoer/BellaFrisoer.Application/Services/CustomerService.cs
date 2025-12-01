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
        private readonly ICustomerConflictChecker _conflictChecker;

        public CustomerService(ICustomerRepository repository, ICustomerConflictChecker conflictChecker)
        {
            _repository = repository;
            _conflictChecker = conflictChecker;
        }

        public async Task<bool> CanCreateCustomerAsync(Customer newCustomer)
        {
            var all = await _repository.GetAllAsync();
            var relevant = all.Where(b => b.EmployeeId == newCustomer.EmployeeId).ToList();
            return !_conflictChecker.HasCustomerConflict(newCustomer, relevant);
        }

        public async Task AddCustomerAsync(Customer Customer)
        {
            await _repository.AddAsync(Customer);
        }

        public async Task<List<Customer>> GetAllAsync() => await _repository.GetAllAsync();
    }
}