using BellaFrisoer.Application.DTOs;
using BellaFrisoer.Application.Interfaces;
using BellaFrisoer.Domain.Models;
using BellaFrisoer.Domain.Services;

namespace BellaFrisoer.Application.CommandHandlers;

public class BookingCommandHandler : IBookingCommandHandler
{
    private readonly IBookingService _bookingService;

    public BookingCommandHandler(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    // Create
    public async Task CreateAsync(BookingCreateDto command, CancellationToken cancellationToken = default)
    {
        await _bookingService.AddBookingAsync(command, cancellationToken);
    }

    // Update
    public async Task UpdateAsync(BookingUpdateDto command, CancellationToken cancellationToken = default)
    {
        await _bookingService.UpdateBookingAsync(command, cancellationToken);
    }

    // Delete
    public async Task DeleteAsync(BookingDeleteDto command, CancellationToken cancellationToken = default)
    {
        var booking = await _bookingService.GetByIdAsync(command.Id, cancellationToken);
        if (booking != null)
        {
            await _bookingService.DeleteBookingAsync(command, cancellationToken);
        }
    }
}