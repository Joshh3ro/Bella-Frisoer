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
    public class CustomerRepository : ICustomerRepository
    {
        private readonly BellaFrisoerWebUiContext _context;

        public CustomerRepository(BellaFrisoerWebUiContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IReadOnlyList<Customer>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Customers
                .AsNoTracking()
                .Include(c => c.Bookings)
                .OrderBy(c => c.LastName)
                .ThenBy(c => c.FirstName)
                .ToListAsync(cancellationToken);
        }

        public async Task<Customer?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Customers
                .Include(c => c.Bookings)
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task AddAsync(Customer customer, CancellationToken cancellationToken = default)
        {
            if (customer is null)
                throw new ArgumentNullException(nameof(customer));

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Customer customer, CancellationToken cancellationToken = default)
        {
            if (customer is null)
                throw new ArgumentNullException(nameof(customer));

            try
            {
                _context.Attach(customer);
                _context.Entry(customer).State = EntityState.Modified;
                await _context.SaveChangesAsync(cancellationToken);
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
            var entity = await _context.Customers.FindAsync(new object[] { id }, cancellationToken);

            if (entity is null)
                return;

            try
            {
                _context.Customers.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);
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
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await GetAllAsync(cancellationToken);
            }

            searchTerm = searchTerm.Trim();

            return await _context.Customers
                .AsNoTracking()
                .Include(c => c.Bookings)
                .Where(c =>
                    (c.FirstName != null && EF.Functions.Like(c.FirstName, $"%{searchTerm}%")) ||
                    (c.LastName != null && EF.Functions.Like(c.LastName, $"%{searchTerm}%")) ||
                    (c.PhoneNumber.ToString().Contains(searchTerm)) ||
                    c.Id.ToString().Contains(searchTerm)
                )
                .OrderBy(c => c.LastName)
                .ThenBy(c => c.FirstName)
                .ToListAsync(cancellationToken);
        }
    }
}
