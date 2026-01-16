using BellaFrisoer.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellaFrisoer.Application.Queries
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IBookingQuery _bookingQuery;

        public InvoiceService(IBookingQuery bookingQuery)
        {
            _bookingQuery = bookingQuery;
        }

        public async Task<string> GenerateInvoiceAsync(int bookingId)
        {
            var booking = await _bookingQuery.GetByIdAsync(bookingId, cancellationToken: default);
            if (booking is null)
                throw new KeyNotFoundException($"Booking with ID {bookingId} not found.");

            string invoiceText = $"""
                Faktura for booking #{booking.Id}

                Kunde: {booking.Customer?.FirstName} {booking.Customer?.LastName}
                Ansat: {booking.Employee?.FirstName} {booking.Employee?.LastName}
                Dato: {booking.BookingDate:yyyy-MM-dd}
                Starttidspunkt: {booking.BookingStartTime:HH:mm}
                Varighed: {booking.BookingDuration} minutter
                Behandling: {booking.Treatment?.Name}
                Pris: {booking.TotalPrice:C}
                ---------------------------
                Bella Frisør
                {DateTime.Now:yyyy-MM-dd HH:mm}
                ---------------------------
                """;

            return invoiceText;
        }
    }
}
