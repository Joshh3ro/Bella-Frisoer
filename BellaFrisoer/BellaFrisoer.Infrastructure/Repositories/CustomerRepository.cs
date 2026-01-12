using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BellaFrisoer.Domain.Models;
using BellaFrisoer.Infrastructure.Data;
using BellaFrisoer.Application.Repositories;

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
                .OrderBy(c => c.LastName)
                .ThenBy(c => c.FirstName)
                .ToListAsync(cancellationToken);
        }

        public async Task<Customer?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            await using var ctx = await _dbFactory.CreateDbContextAsync(cancellationToken);
            return await ctx.Customers
                .Include(c => c.Bookings)
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task AddAsync(Customer customer, CancellationToken cancellationToken = default)
        {
            if (customer is null)
                throw new ArgumentNullException(nameof(customer));

            await using var ctx = await _dbFactory.CreateDbContextAsync(cancellationToken);
            ctx.Customers.Add(customer);
            await ctx.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Customer customer, CancellationToken cancellationToken = default)
        {
            if (customer is null)
                throw new ArgumentNullException(nameof(customer));

            await using var ctx = await _dbFactory.CreateDbContextAsync(cancellationToken);

            try
            {
                ctx.Attach(customer);
                ctx.Entry(customer).State = EntityState.Modified;
                await ctx.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new ApplicationException(
                    "Kunden kunne ikke opdateres, da den blev ændret af en anden bruger. Prøv venligst at opdatere siden og prøv igen.",
                    ex);
            }
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            await using var ctx = await _dbFactory.CreateDbContextAsync(cancellationToken);
            var entity = await ctx.Customers.FindAsync(new object[] { id }, cancellationToken);

            if (entity is null)
                return;

            try
            {
                ctx.Customers.Remove(entity);
                await ctx.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new ApplicationException(
                    "Kunden kunne ikke slettes, da den blev ændret af en anden bruger. Prøv venligst at opdatere siden og prøv igen.",
                    ex);
            }
        }

        public async Task<IReadOnlyList<Customer>> FilterCustomersAsync(string searchTerm, CancellationToken cancellationToken = default)
        {
            await using var ctx = await _dbFactory.CreateDbContextAsync(cancellationToken);

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await ctx.Customers
                    .AsNoTracking()
                    .Include(c => c.Bookings)
                    .OrderBy(c => c.LastName)
                    .ThenBy(c => c.FirstName)
                    .ToListAsync(cancellationToken);
            }

            searchTerm = searchTerm.Trim().ToLower();

            return await ctx.Customers
                .AsNoTracking()
                .Include(c => c.Bookings)
                .Where(c =>
                    (c.FirstName != null && EF.Functions.Like(c.FirstName.ToLower(), $"%{searchTerm}%")) ||
                    (c.LastName != null && EF.Functions.Like(c.LastName.ToLower(), $"%{searchTerm}%")) ||
                    (c.PhoneNumber != null && c.PhoneNumber.ToString().Contains(searchTerm)) ||
                    c.Id.ToString().Contains(searchTerm)
                )
                .OrderBy(c => c.LastName)
                .ThenBy(c => c.FirstName)
                .ToListAsync(cancellationToken);
        }
    }
}
