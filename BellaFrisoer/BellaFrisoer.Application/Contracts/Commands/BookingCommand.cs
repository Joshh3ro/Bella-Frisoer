using BellaFrisoer.Application.Repositories;
using BellaFrisoer.Domain.Models;
using System;
using System.Threading.Tasks;

namespace BellaFrisoer.Application.Contracts.Commands
{
    public class BookingCommand : IBookingCommand
    {
        private readonly IBookingRepository _repository;
        public BookingCommand(IBookingRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task CreateAsync(BookingCreateCommandDto dto)
        {
            if (dto is null) throw new ArgumentNullException(nameof(dto));
            if (dto.CustomerId <= 0) throw new ArgumentException("CustomerId must be provided.", nameof(dto.CustomerId));
            if (dto.EmployeeId <= 0) throw new ArgumentException("EmployeeId must be provided.", nameof(dto.EmployeeId));
            if (dto.TreatmentId <= 0) throw new ArgumentException("TreatmentId must be provided.", nameof(dto.TreatmentId));
            if (dto.DurationMinutes <= 0) throw new ArgumentException("DurationMinutes must be greater than zero.", nameof(dto.DurationMinutes));

            // Create minimal domain entities with ids (domain factory requires non-null objects).
            var customer = new Customer { Id = dto.CustomerId };
            var employee = new Employee { Id = dto.EmployeeId };
            var treatment = new Treatment { Id = dto.TreatmentId, Duration = dto.DurationMinutes, Name = string.Empty, Price = 0m };

            var duration = TimeSpan.FromMinutes(dto.DurationMinutes);

            // Use domain factory to apply domain validation/rules
            var booking = Booking.Create(customer, employee, treatment, dto.BookingDate, dto.BookingStartTime, duration);

            // persist
            await _repository.AddAsync(booking);
        }
    }
}
