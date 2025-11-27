csharp BellaFrisoer.Application\Interfaces\IBookingService.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using BellaFrisoer.Domain.Models;

namespace BellaFrisoer.Application.Interfaces
{
    public interface IBookingService
    {
        Task<bool> CanCreateBookingAsync(Booking newBooking);
        Task AddBookingAsync(Booking booking);
        Task<List<Booking>> GetAllAsync();
    }
}