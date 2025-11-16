using BellaFrisoer.Domain.Interfaces;
using BellaFrisoer.Domain.Models;
using BellaFrisoer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class BookingRepository : IBookingRepository
{
    private readonly BellaFrisoerWebUiContext _context;

    public BookingRepository(BellaFrisoerWebUiContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Booking>> GetAllAsync()
        => await _context.Bookings.Include(b => b.Customer).ToListAsync();

    public async Task<Booking?> GetByIdAsync(int id)
        => await _context.Bookings.Include(b => b.Customer).FirstOrDefaultAsync(b => b.Id == id);

    public async Task AddAsync(Booking booking)
    {
        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();
    }
    // Other CRUD methods
}