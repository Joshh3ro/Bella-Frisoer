using BellaFrisoer.Domain.Interfaces;
using BellaFrisoer.Domain.Models;
using Microsoft.EntityFrameworkCore;

public class BookingRepository : IBookingRepository
{
    private readonly DbContext _context;

    public BookingRepository(DbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Booking>> GetAllAsync()
        => await _context.Set<Booking>().Include(b => b.Customer).ToListAsync();

    public async Task<Booking?> GetByIdAsync(int id)
        => await _context.Set<Booking>().Include(b => b.Customer).FirstOrDefaultAsync(b => b.Id == id);

    public async Task AddAsync(Booking booking)
    {
        await _context.Set<Booking>().AddAsync(booking);
        await _context.SaveChangesAsync();
    }

    // Other CRUD methods
}