using BellaCuts.Application.Services;
using BellaCuts.Domain.Entities;
using BellaCuts.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BellaCuts.Infrastructure.Services;

public class CustomerService : ICustomerService
{
    private readonly BellaCutsDbContext _db;

    public CustomerService(BellaCutsDbContext db) => _db = db;

    public async Task<IEnumerable<Customer>> GetAllAsync(CancellationToken cancellation = default) =>
        await _db.Customers.AsNoTracking().ToListAsync(cancellation);

    public async Task<Customer?> GetAsync(Guid id, CancellationToken cancellation = default) =>
        await _db.Customers.FindAsync(new object[] { id }, cancellation);

    public async Task AddAsync(Customer customer, CancellationToken cancellation = default)
    {
        _db.Customers.Add(customer);
        await _db.SaveChangesAsync(cancellation);
    }

    public async Task UpdateAsync(Customer customer, CancellationToken cancellation = default)
    {
        _db.Customers.Update(customer);
        await _db.SaveChangesAsync(cancellation);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellation = default)
    {
        var entity = await _db.Customers.FindAsync(new object[] { id }, cancellation);
        if (entity is null) return;
        _db.Customers.Remove(entity);
        await _db.SaveChangesAsync(cancellation);
    }
}