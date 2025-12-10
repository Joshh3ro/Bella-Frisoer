using Microsoft.EntityFrameworkCore;
using BellaFrisoer.Infrastructure.Data;
using BellaFrisoer.Application.Interfaces;
using BellaFrisoer.Domain.Models;
// NOTE: Repository er vores database logik,

namespace BellaFrisoer.Infrastructure.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly IDbContextFactory<BellaFrisoerWebUiContext> _dbFactory;

        public BookingRepository(IDbContextFactory<BellaFrisoerWebUiContext> dbFactory)
        {
            _dbFactory = dbFactory ?? throw new ArgumentNullException(nameof(dbFactory));
        }

        public async Task<IReadOnlyList<Booking>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            await using var ctx = await _dbFactory.CreateDbContextAsync(cancellationToken);
            // Read-only query: AsNoTracking increases performance when not updating entities.
            var bookings = await ctx.Bookings
                .AsNoTracking()
                .Include(b => b.Customer)
                .Include(b => b.Employee)
                .Include(b => b.Treatment)
                .OrderBy(b => b.BookingDate)
                .ToListAsync(cancellationToken);

            return bookings;
        }

        public async Task<IReadOnlyList<Booking>> GetByEmployeeIdAsync(int employeeId, CancellationToken cancellationToken = default)
        {
            await using var ctx = await _dbFactory.CreateDbContextAsync(cancellationToken);

            var bookings = await ctx.Bookings
                .AsNoTracking()
                .Where(b => b.EmployeeId == employeeId)
                .Include(b => b.Customer)
                .Include(b => b.Employee)
                .Include(b => b.Treatment)
                .OrderBy(b => b.BookingDate)
                .ToListAsync(cancellationToken);

            return bookings;
        }

        // New: efficient retrieval for bookings of an employee on a particular date.
        public async Task<IReadOnlyList<Booking>> GetByEmployeeIdAndDateAsync(int employeeId, DateTime bookingDate, CancellationToken cancellationToken = default)
        {
            await using var ctx = await _dbFactory.CreateDbContextAsync(cancellationToken);

            var dateOnly = bookingDate.Date;

            var bookings = await ctx.Bookings
                .AsNoTracking()
                .Where(b => b.EmployeeId == employeeId && b.BookingDate == dateOnly)
                .Include(b => b.Customer)
                .Include(b => b.Employee)
                .Include(b => b.Treatment)
                .OrderBy(b => b.BookingStartTime)
                .ToListAsync(cancellationToken);

            return bookings;
        }

        public async Task<Booking?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            await using var ctx = await _dbFactory.CreateDbContextAsync(cancellationToken);

            return await ctx.Bookings
                .AsNoTracking()
                .Include(b => b.Customer)
                .Include(b => b.Employee)
                .Include(b => b.Treatment)
                .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
        }

        public async Task AddAsync(Booking booking, CancellationToken cancellationToken = default)
        {
            if (booking is null) throw new ArgumentNullException(nameof(booking));

            await using var ctx = await _dbFactory.CreateDbContextAsync(cancellationToken);

            // Attach scalar keys rather than full objects if provided only as ids, avoiding accidental inserts
            if (booking.Customer is not null)
            {
                ctx.Attach(booking.Customer);
            }

            if (booking.Employee is not null)
            {
                ctx.Attach(booking.Employee);
            }
            if (booking.Treatment is not null)
            {
                ctx.Attach(booking.Treatment);
            }

            ctx.Bookings.Add(booking);
            await ctx.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Booking booking, CancellationToken cancellationToken = default)
        {
            if (booking is null) throw new ArgumentNullException(nameof(booking));

            await using var ctx = await _dbFactory.CreateDbContextAsync(cancellationToken);
            ctx.Attach(booking).State = EntityState.Modified;
            
            await ctx.SaveChangesAsync(cancellationToken);
        }
        
        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            await using var ctx = await _dbFactory.CreateDbContextAsync(cancellationToken);
            var entity = await ctx.Bookings.FindAsync(new object[] { id }, cancellationToken);
            if (entity is null) return;
            ctx.Bookings.Remove(entity);
        }
        public async Task<IReadOnlyList<Booking>> FilterBookingsAsync(string searchTerm, CancellationToken cancellationToken = default)
        {
            await using var ctx = await _dbFactory.CreateDbContextAsync(cancellationToken);

            // Base query including all related entities
            var query = ctx.Bookings
                .AsNoTracking()
                .Include(b => b.Customer)
                .Include(b => b.Employee)
                .Include(b => b.Treatment)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.Trim().ToLower();

                query = query.Where(b =>
                    (b.Customer.FirstName != null && b.Customer.FirstName.ToLower().Contains(searchTerm)) ||
                    (b.Customer.LastName != null && b.Customer.LastName.ToLower().Contains(searchTerm)) ||
                    (b.Customer.PhoneNumber != null && b.Customer.PhoneNumber.ToString().Contains(searchTerm)) ||
                    (b.Employee.FirstName != null && b.Employee.FirstName.ToLower().Contains(searchTerm)) ||
                    (b.Employee.LastName != null && b.Employee.LastName.ToLower().Contains(searchTerm)) ||
                    (b.Treatment.Name != null && b.Treatment.Name.ToLower().Contains(searchTerm)) ||
                    b.Id.ToString().Contains(searchTerm)
                );
            }

            return await query
                .OrderBy(b => b.BookingDate)
                .ThenBy(b => b.BookingStartTime)
                .ToListAsync(cancellationToken);
        }

    }
}