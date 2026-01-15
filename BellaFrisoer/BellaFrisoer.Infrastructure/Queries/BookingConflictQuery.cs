using BellaFrisoer.Domain.Models;
using BellaFrisoer.Domain.Queries;
using BellaFrisoer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BellaFrisoer.Infrastructure.Queries
{
    /// <summary>
    /// Implementering af vores BookingConflictQuery (read-only, infrastructure layer)
    /// </summary>
    public class BookingConflictQuery : IBookingConflictQuery
    {
        private readonly BellaFrisoerWebUiContext _context;

        public BookingConflictQuery(BellaFrisoerWebUiContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Booking>> GetBookingsByEmployeeAndDateAsync(
            int employeeId, 
            DateTime date, 
            CancellationToken cancellationToken = default)
        {
            return await _context.Bookings
                .AsNoTracking()
                .Where(b => b.Employee.Id == employeeId && b.BookingDate.Date == date.Date)
                .Include(b => b.Customer)
                .Include(b => b.Employee)
                .Include(b => b.Treatment)
                .ToListAsync(cancellationToken);
        }
    }
}