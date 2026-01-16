using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BellaFrisoer.Application.Interfaces;
using BellaFrisoer.Domain.Models;
using BellaFrisoer.Infrastructure.Data;

namespace BellaFrisoer.Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly BellaFrisoerWebUiContext _context;

        public EmployeeRepository(BellaFrisoerWebUiContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IReadOnlyList<Employee>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Employees
                .AsNoTracking()
                .OrderBy(e => e.LastName)
                .ThenBy(e => e.FirstName)
                .ToListAsync(cancellationToken);
        }

        public async Task<Employee?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Employees
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }

        public async Task AddAsync(Employee employee, CancellationToken cancellationToken = default)
        {
            if (employee is null)
                throw new ArgumentNullException(nameof(employee));

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Employee employee, CancellationToken cancellationToken = default)
        {
            if (employee is null)
                throw new ArgumentNullException(nameof(employee));

            try
            {
                _context.Attach(employee);
                _context.Entry(employee).State = EntityState.Modified;
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new ApplicationException(
                    "Medarbejderen kunne ikke opdateres, da den blev ændret af en anden bruger. Prøv venligst at opdatere siden og prøv igen.",
                    ex);
            }
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _context.Employees.FindAsync(new object[] { id }, cancellationToken);

            if (entity is null)
                return;

            try
            {
                _context.Employees.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new ApplicationException(
                    "Medarbejderen kunne ikke slettes, da den blev ændret af en anden bruger. Prøv venligst at opdatere siden og prøv igen.",
                    ex);
            }
        }

        public async Task<List<Employee>> FilterEmployeesAsync(string searchTerm, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await _context.Employees
                    .AsNoTracking()
                    .OrderBy(e => e.LastName)
                    .ThenBy(e => e.FirstName)
                    .ToListAsync(cancellationToken);
            }

            searchTerm = searchTerm.Trim();

            return await _context.Employees
                .AsNoTracking()
                .Where(e =>
                    EF.Functions.Like(e.FirstName ?? string.Empty, $"%{searchTerm}%") ||
                    EF.Functions.Like(e.LastName ?? string.Empty, $"%{searchTerm}%") ||
                    EF.Functions.Like(e.Email ?? string.Empty, $"%{searchTerm}%") ||
                    (e.PhoneNumber.ToString().Contains(searchTerm)) ||
                    e.Id.ToString().Contains(searchTerm) ||
                    (e.Qualifications != null && EF.Functions.Like(e.Qualifications, $"%{searchTerm}%"))
                )
                .OrderBy(e => e.LastName)
                .ThenBy(e => e.FirstName)
                .ToListAsync(cancellationToken);
        }
    }
}
