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

            ctx.Bookings.Add(booking);
            await ctx.SaveChangesAsync(cancellationToken);
        }
    }
}