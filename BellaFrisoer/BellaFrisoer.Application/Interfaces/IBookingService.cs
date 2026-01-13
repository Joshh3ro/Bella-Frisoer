using BellaFrisoer.Domain.Models;
using BellaFrisoer.Domain.Models.Discounts;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BellaFrisoer.Application.Interfaces
{
    public interface IBookingService
    {
        Task<bool> CanCreateBookingAsync(Booking newBooking, CancellationToken cancellationToken = default);
        Task AddBookingAsync(Booking booking, CancellationToken cancellationToken = default);
        Task UpdateBookingAsync(Booking booking, CancellationToken cancellationToken = default);
        Task DeleteBookingAsync(Booking booking, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Booking>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Booking?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<Booking>> FilterBookingsAsync(string searchTerm, CancellationToken cancellationToken = default);
        bool HasBookingConflict(Booking otherBooking, IEnumerable<Booking> existingBookings);
        IDiscountStrategy? GetDiscountStrategyForCustomerTotalBookings(Customer customer);

        decimal CalculatePrice(Booking booking, Employee? employee, Treatment? treatment, Customer? customer = null);


    }
}
