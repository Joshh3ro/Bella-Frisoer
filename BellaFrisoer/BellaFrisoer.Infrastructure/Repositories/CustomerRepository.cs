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
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IDbContextFactory<BellaFrisoerWebUiContext> _dbFactory;

        public CustomerRepository(IDbContextFactory<BellaFrisoerWebUiContext> dbFactory)
        {
            _dbFactory = dbFactory ?? throw new ArgumentNullException(nameof(dbFactory));
        }

        public async Task<IReadOnlyList<Customer>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            await using var ctx = await _dbFactory.CreateDbContextAsync(cancellationToken);
            return await ctx.Customers
                .AsNoTracking()
                .Include(c => c.Bookings)
                .ToListAsync(cancellationToken);
        }

        public async Task<Customer?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            await using var ctx = await _dbFactory.CreateDbContextAsync(cancellationToken);
            return await ctx.Customers
                .AsNoTracking()
                .Include(c => c.Bookings)
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task AddAsync(Customer customer, CancellationToken cancellationToken = default)
        {
            if (customer is null) throw new ArgumentNullException(nameof(customer));
            await using var ctx = await _dbFactory.CreateDbContextAsync(cancellationToken);
            ctx.Customers.Add(customer);
            await ctx.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Customer customer, CancellationToken cancellationToken = default)
        {
            if (customer is null) throw new ArgumentNullException(nameof(customer));
            await using var ctx = await _dbFactory.CreateDbContextAsync(cancellationToken);
            ctx.Customers.Update(customer);
            await ctx.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            await using var ctx = await _dbFactory.CreateDbContextAsync(cancellationToken);
            var entity = await ctx.Customers.FindAsync(new object[] { id }, cancellationToken);
            if (entity is null) return;
            ctx.Customers.Remove(entity);
            await ctx.SaveChangesAsync(cancellationToken);
        }
    }
}