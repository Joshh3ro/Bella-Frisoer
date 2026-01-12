using BellaFrisoer.Application.Repositories;
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

        

        
    }
}
