using System;
using System.Threading.Tasks;

namespace BellaFrisoer.Application.Contracts.Commands
{
    public interface IBookingCommand
    {
        Task CreateAsync(BookingCreateCommandDto dto);
    }

    public class BookingCreateCommandDto
    {
        public int CustomerId { get; set; }
        public int EmployeeId { get; set; }
        public int TreatmentId { get; set; }

        // Date (date part) for the booking
        public DateTime BookingDate { get; set; }

        // Start time for the booking (uses TimeOnly like domain model)
        public TimeOnly BookingStartTime { get; set; }

        // Duration in minutes
        public int DurationMinutes { get; set; }
    }
}
