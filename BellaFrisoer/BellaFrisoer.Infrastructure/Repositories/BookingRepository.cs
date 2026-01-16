using BellaFrisoer.Application.Interfaces;
using BellaFrisoer.Domain.Models;
using BellaFrisoer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BellaFrisoer.Infrastructure.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly BellaFrisoerWebUiContext _context;

        public BookingRepository(BellaFrisoerWebUiContext context)
        {
            _context = context;
        }


        public async Task AddAsync(Booking booking, CancellationToken cancellationToken = default)
        {
            await _context.Bookings.AddAsync(booking, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Booking booking, CancellationToken cancellationToken = default)
        {
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var booking = await _context.Bookings.FindAsync(new object[] { id }, cancellationToken);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<Booking> LoadAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Employee)
                .Include(b => b.Treatment)
                .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
        }

    }
}
