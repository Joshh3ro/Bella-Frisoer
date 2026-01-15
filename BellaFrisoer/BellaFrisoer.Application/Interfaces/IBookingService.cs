using BellaFrisoer.Application.DTOs;
using BellaFrisoer.Domain.Models;
using BellaFrisoer.Domain.Models.Discounts;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BellaFrisoer.Application.Interfaces
{
    public interface IBookingService
    {
        Task AddBookingAsync(BookingCreateDto dto, CancellationToken cancellationToken = default);
        Task UpdateBookingAsync(BookingUpdateDto dto, CancellationToken cancellationToken = default);
        Task DeleteBookingAsync(Booking booking, CancellationToken cancellationToken = default);
        IDiscountStrategy? GetDiscountStrategyForCustomerTotalBookings(Customer customer);

    }
}
