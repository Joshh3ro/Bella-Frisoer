using Microsoft.EntityFrameworkCore;
using BellaFrisoer.Infrastructure.Data;
using BellaFrisoer.Application.Interfaces;
using BellaFrisoer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
                .Include(b => b.Customer)
                .Include(b => b.Employee)
                .Include(b => b.Treatment)
                .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
        }

        public async Task AddAsync(Booking booking, CancellationToken cancellationToken = default)
        {
            if (booking is null)
                throw new ArgumentNullException(nameof(booking));

            await using var ctx = await _dbFactory.CreateDbContextAsync(cancellationToken);

            // Gem kun ID'er for relationer for at undgå uventet indsætning
            var customerId = booking.Customer?.Id;
            var employeeId = booking.Employee?.Id;
            var treatmentId = booking.Treatment?.Id;

            // Nulstil navigation properties for at undgå uventet adfærd
            booking.Customer = null;
            booking.Employee = null;
            booking.Treatment = null;

            // Tilføj booking
            ctx.Bookings.Add(booking);
            await ctx.SaveChangesAsync(cancellationToken);

            // Genopret relationer efter gem, hvis nødvendigt
            if (customerId.HasValue)
                booking.Customer = await ctx.Customers.FindAsync(new object[] { customerId.Value }, cancellationToken);
            if (employeeId.HasValue)
                booking.Employee = await ctx.Employees.FindAsync(new object[] { employeeId.Value }, cancellationToken);
            if (treatmentId.HasValue)
                booking.Treatment = await ctx.Treatments.FindAsync(new object[] { treatmentId.Value }, cancellationToken);
        }

        public async Task UpdateAsync(Booking booking, CancellationToken cancellationToken = default)
        {
            if (booking is null)
                throw new ArgumentNullException(nameof(booking));

            await using var ctx = await _dbFactory.CreateDbContextAsync(cancellationToken);

            try
            {
                // Håndter kun booking-entiteten for at undgå uventet opdatering af relationer
                ctx.Attach(booking);
                ctx.Entry(booking).State = EntityState.Modified;
                await ctx.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Kast en custom exception, hvis RowVersion ikke matcher
                throw new ApplicationException(
                    "Bookingen kunne ikke opdateres, da den blev ændret af en anden bruger. Prøv venligst at opdatere siden og prøv igen.",
                    ex);
            }
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            await using var ctx = await _dbFactory.CreateDbContextAsync(cancellationToken);
            var entity = await ctx.Bookings.FindAsync(new object[] { id }, cancellationToken);

            if (entity is null)
                return;

            try
            {
                ctx.Bookings.Remove(entity);
                await ctx.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Kast en custom exception, hvis der er en konflikt ved sletning
                throw new ApplicationException(
                    "Bookingen kunne ikke slettes, da den blev ændret af en anden bruger. Prøv venligst at opdatere siden og prøv igen.",
                    ex);
            }
        }

        public async Task<IReadOnlyList<Booking>> FilterBookingsAsync(string searchTerm, CancellationToken cancellationToken = default)
        {
            await using var ctx = await _dbFactory.CreateDbContextAsync(cancellationToken);
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
