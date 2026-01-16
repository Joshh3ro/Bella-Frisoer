using BellaFrisoer.Application.DTOs;
using BellaFrisoer.Domain.Models;

namespace BellaFrisoer.Application.Interfaces;

public interface IBookingCommandHandler
{
    Task CreateAsync(BookingCreateDto command, CancellationToken cancellationToken = default);
    Task UpdateAsync(BookingUpdateDto command, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
