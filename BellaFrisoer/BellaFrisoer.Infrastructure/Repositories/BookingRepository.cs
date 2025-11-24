using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BellaFrisoer.Domain.Interfaces;
using BellaFrisoer.Domain.Models;
using BellaFrisoer.Infrastructure.Data;

namespace BellaFrisoer.Infrastructure.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly IDbContextFactory<BellaFrisoerWebUiContext> _dbFactory;

    public BookingRepository(IDbContextFactory<BellaFrisoerWebUiContext> dbFactory)
    {
        _dbFactory = dbFactory;
    }

    public async Task<List<Booking>> GetAllAsync()
    {
        using var ctx = _dbFactory.CreateDbContext();
        return await ctx.Bookings
            .Include(b => b.Customer)
            .Include(b => b.Employee)
            .OrderBy(b => b.BookingDate)
            .ToListAsync();
    }

    public async Task<Booking?> GetByIdAsync(int id)
    {
        using var ctx = _dbFactory.CreateDbContext();
        return await ctx.Bookings
            .Include(b => b.Customer)
            .Include(b => b.Employee)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task AddAsync(Booking booking)
    {
        using var ctx = _dbFactory.CreateDbContext();
        ctx.Bookings.Add(booking);
        await ctx.SaveChangesAsync();
    }
}