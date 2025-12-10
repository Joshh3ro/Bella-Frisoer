using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BellaFrisoer.Domain.Models;
using BellaFrisoer.Application.Interfaces;
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
                .ToListAsync(cancellationToken);
        }

        public async Task<Employee?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            await using var ctx = await _dbFactory.CreateDbContextAsync(cancellationToken);
            return await ctx.Employees
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }

        public async Task AddAsync(Employee employee, CancellationToken cancellationToken = default)
        {
            if (employee is null) throw new ArgumentNullException(nameof(employee));
            await using var ctx = await _dbFactory.CreateDbContextAsync(cancellationToken);
            ctx.Employees.Add(employee);
            await ctx.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Employee employee, CancellationToken cancellationToken = default)
        {
            if (employee is null) throw new ArgumentNullException(nameof(employee));
            await using var ctx = await _dbFactory.CreateDbContextAsync(cancellationToken);
            ctx.Employees.Update(employee);
            await ctx.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            await using var ctx = await _dbFactory.CreateDbContextAsync(cancellationToken);
            var entity = await ctx.Employees.FindAsync(new object[] { id }, cancellationToken);
            if (entity is null) return;
            ctx.Employees.Remove(entity);
            await ctx.SaveChangesAsync(cancellationToken);
        }
        public async Task<List<Employee>> FilterEmployeesAsync(string searchTerm, CancellationToken cancellationToken = default)
        {
            await using var ctx = await _dbFactory.CreateDbContextAsync(cancellationToken);

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await ctx.Employees
                    .AsNoTracking()
                    .Include(e => e.Qualifications)
                    .ToListAsync(cancellationToken);
            }

            searchTerm = $"%{searchTerm.Trim()}%";

            return await ctx.Employees
                .AsNoTracking()
                .Include(e => e.Qualifications)
                .Where(e =>
                    EF.Functions.Like(e.FirstName ?? string.Empty, searchTerm) ||
                    EF.Functions.Like(e.LastName ?? string.Empty, searchTerm) ||
                    EF.Functions.Like(e.PhoneNumber.ToString(), searchTerm) ||
                    EF.Functions.Like(e.Email ?? string.Empty, searchTerm)
                )
                .ToListAsync(cancellationToken);
        }


    }
}