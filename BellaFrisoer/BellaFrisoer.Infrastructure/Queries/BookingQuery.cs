using BellaFrisoer.Application.Queries;
using BellaFrisoer.Domain.Models;
using BellaFrisoer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellaFrisoer.Infrastructure.Queries
{
    public class BookingQuery : IBookingQuery
    {
        private readonly BellaFrisoerWebUiContext _context;

        public BookingQuery(BellaFrisoerWebUiContext context)
        {
            _context = context;
        }

        public async Task<List<Booking>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Bookings
                .AsNoTracking()
                .Include(b => b.Customer)
                .Include(b => b.Employee)
                .Include(b => b.Treatment)
                .ToListAsync(cancellationToken);
        }

        public async Task<Booking?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Bookings
                .AsNoTracking()
                .Include(b => b.Customer)
                .Include(b => b.Employee)
                .Include(b => b.Treatment)
                .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
        }

        public async Task<List<Booking>> GetByEmployeeIdAndDateAsync(int EmployeeId, DateTime date, CancellationToken cancellationToken = default)
        {
            return await _context.Bookings
                .AsNoTracking()
                .Where(b => b.Employee != null && b.Employee.Id == EmployeeId && b.BookingDate.Date == date.Date)
                .Include(b => b.Customer)
                .Include(b => b.Employee)
                .Include(b => b.Treatment)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Booking>> FilterBookingsAsync(string searchTerm, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return new List<Booking>();

            return await _context.Bookings
                .AsNoTracking()
                .Where(b =>
                    (b.Customer != null && EF.Functions.Like(b.Customer.FirstName, $"%{searchTerm}%")) ||
                    (b.Customer != null && EF.Functions.Like(b.Customer.LastName, $"%{searchTerm}%")) ||
                    (b.Employee != null && EF.Functions.Like(b.Employee.FirstName, $"%{searchTerm}%")) ||
                    (b.Employee != null && EF.Functions.Like(b.Employee.LastName, $"%{searchTerm}%")) ||
                    (b.Treatment != null && EF.Functions.Like(b.Treatment.Name, $"%{searchTerm}%")))
                .Include(b => b.Customer)
                .Include(b => b.Employee)
                .Include(b => b.Treatment)
                .ToListAsync(cancellationToken);
        }

    }
}
