using BellaFrisoer.Application.Interfaces;
using BellaFrisoer.Domain.Models;
using BellaFrisoer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BellaFrisoer.Infrastructure.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly IDbContextFactory<BellaFrisoerWebUiContext> _contextFactory;

        public BookingRepository(IDbContextFactory<BellaFrisoerWebUiContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<List<Booking>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            return await context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Employee)
                .Include(b => b.Treatment)
                .ToListAsync(cancellationToken);
        }

        public async Task<Booking?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            return await context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Employee)
                .Include(b => b.Treatment)
                .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
        }

        public async Task AddAsync(Booking booking, CancellationToken cancellationToken = default)
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            await context.Bookings.AddAsync(booking, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Booking booking, CancellationToken cancellationToken = default)
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            context.Bookings.Update(booking);
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var booking = await context.Bookings.FindAsync(new object[] { id }, cancellationToken);
            if (booking != null)
            {
                context.Bookings.Remove(booking);
                await context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<List<Booking>> GetByEmployeeIdAndDateAsync(int EmployeeId, DateTime date, CancellationToken cancellationToken = default)
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            return await context.Bookings
                .Where(b => b.Employee.Id == EmployeeId && b.BookingDate.Date == date.Date)
                .Include(b => b.Customer)
                .Include(b => b.Employee)
                .Include(b => b.Treatment)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Booking>> FilterBookingsAsync(string searchTerm, CancellationToken cancellationToken = default)
        {
            using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            return await context.Bookings
                .Where(b =>
                    (b.Customer != null && b.Customer.FirstName.Contains(searchTerm)) ||
                    (b.Customer != null && b.Customer.LastName.Contains(searchTerm)) ||
                    (b.Employee != null && b.Employee.FirstName.Contains(searchTerm)) ||
                    (b.Employee != null && b.Employee.LastName.Contains(searchTerm)) ||
                    (b.Treatment != null && b.Treatment.Name.Contains(searchTerm)))
                .Include(b => b.Customer)
                .Include(b => b.Employee)
                .Include(b => b.Treatment)
                .ToListAsync(cancellationToken);
        }
    }
}
