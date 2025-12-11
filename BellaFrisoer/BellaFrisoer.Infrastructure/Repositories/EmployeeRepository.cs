using System;
using System.Collections.Generic;
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
        private readonly IDbContextFactory<BellaFrisoerWebUiContext> _dbFactory;

        public EmployeeRepository(IDbContextFactory<BellaFrisoerWebUiContext> dbFactory)
        {
            _dbFactory = dbFactory ?? throw new ArgumentNullException(nameof(dbFactory));
        }

        public async Task<IReadOnlyList<Employee>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            await using var ctx = await _dbFactory.CreateDbContextAsync(cancellationToken);
            return await ctx.Employees
                .AsNoTracking()
                .OrderBy(e => e.LastName)
                .ThenBy(e => e.FirstName)
                .ToListAsync(cancellationToken);
        }

        public async Task<Employee?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            await using var ctx = await _dbFactory.CreateDbContextAsync(cancellationToken);
            return await ctx.Employees
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }

        public async Task AddAsync(Employee employee, CancellationToken cancellationToken = default)
        {
            if (employee is null)
                throw new ArgumentNullException(nameof(employee));

            await using var ctx = await _dbFactory.CreateDbContextAsync(cancellationToken);
            ctx.Employees.Add(employee);
            await ctx.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Employee employee, CancellationToken cancellationToken = default)
        {
            if (employee is null)
                throw new ArgumentNullException(nameof(employee));

            await using var ctx = await _dbFactory.CreateDbContextAsync(cancellationToken);

            try
            {
                ctx.Attach(employee);
                ctx.Entry(employee).State = EntityState.Modified;
                await ctx.SaveChangesAsync(cancellationToken);
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
            await using var ctx = await _dbFactory.CreateDbContextAsync(cancellationToken);
            var entity = await ctx.Employees.FindAsync(new object[] { id }, cancellationToken);

            if (entity is null)
                return;

            try
            {
                ctx.Employees.Remove(entity);
                await ctx.SaveChangesAsync(cancellationToken);
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
            await using var ctx = await _dbFactory.CreateDbContextAsync(cancellationToken);

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await ctx.Employees
                    .AsNoTracking()
                    .OrderBy(e => e.LastName)
                    .ThenBy(e => e.FirstName)
                    .ToListAsync(cancellationToken);
            }

            searchTerm = searchTerm.Trim();

            return await ctx.Employees
                .AsNoTracking()
                .Where(e =>
                    EF.Functions.Like(e.FirstName ?? string.Empty, $"%{searchTerm}%") ||
                    EF.Functions.Like(e.LastName ?? string.Empty, $"%{searchTerm}%") ||
                    EF.Functions.Like(e.Email ?? string.Empty, $"%{searchTerm}%") ||
                    (e.PhoneNumber != null && e.PhoneNumber.ToString().Contains(searchTerm)) ||
                    e.Id.ToString().Contains(searchTerm) ||
                    (e.Qualifications != null && e.Qualifications.Contains(searchTerm)) // Søg i Qualifications (string)
                )
                .OrderBy(e => e.LastName)
                .ThenBy(e => e.FirstName)
                .ToListAsync(cancellationToken);
        }
    }
}
