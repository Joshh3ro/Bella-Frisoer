using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BellaFrisoer.Application.Interfaces;
using BellaFrisoer.Domain.Models;

namespace BellaFrisoer.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repository;

        public EmployeeService(IEmployeeRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public Task<bool> CanCreateEmployeeAsync(Employee newEmployee)
        {
            // Example business rule: require a first name. Adjust as needed.
            var canCreate = newEmployee is not null && !string.IsNullOrWhiteSpace(newEmployee.FirstName);
            return Task.FromResult(canCreate);
        }

        public async Task AddEmployeeAsync(Employee employee)
        {
            if (employee is null) throw new ArgumentNullException(nameof(employee));
            await _repository.AddAsync(employee);
        }

        public async Task<List<Employee>> GetAllAsync()
        {
            var employees = await _repository.GetAllAsync();
            return employees.ToList();
        }
    }
}