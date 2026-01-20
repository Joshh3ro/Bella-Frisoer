using BellaFrisoer.Application.Interfaces;
using BellaFrisoer.Domain.Models;
using BellaFrisoer.Domain.Models.Discounts;
using BellaFrisoer.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BellaFrisoer.Application.Queries;
using BellaFrisoer.Domain.Services;

namespace BellaFrisoer.Application.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _repository;
        private readonly IBookingPriceService _bookingPriceService;
        private readonly ICustomerRepository _customerRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ITreatmentRepository _treatmentRepository;
        private readonly IBookingQuery _bookingQuery;
        private readonly IBookingConflictChecker _bookingConflictChecker;

        public BookingService(
            IBookingRepository repository,
            IBookingPriceService bookingPriceService,
            ICustomerRepository customerRepository,
            IEmployeeRepository employeeRepository,
            ITreatmentRepository treatmentRepository,
            IBookingQuery bookingQuery,
            IBookingConflictChecker bookingConflictChecker)
        {
            _repository = repository;
            _bookingPriceService = bookingPriceService;
            _customerRepository = customerRepository;
            _employeeRepository = employeeRepository;
            _bookingQuery = bookingQuery;
            _treatmentRepository = treatmentRepository;
            _bookingConflictChecker = bookingConflictChecker;
        }

        public async Task<IReadOnlyList<Booking>> GetAllAsync(CancellationToken cancellationToken = default)
            => await _bookingQuery.GetAllAsync(cancellationToken);

        public async Task<Booking?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
            => await _bookingQuery.GetByIdAsync(id, cancellationToken);

        public async Task<List<Booking>> FilterBookingsAsync(string searchTerm, CancellationToken cancellationToken = default)
            => await _bookingQuery.FilterBookingsAsync(searchTerm, cancellationToken);


        public async Task AddBookingAsync(BookingCreateDto dto, CancellationToken cancellationToken = default)
        {
            if (dto is null)
                throw new ArgumentNullException(nameof(dto));

            // Load related entities
            var customer = await _customerRepository.GetByIdAsync(dto.CustomerId, cancellationToken)
                ?? throw new KeyNotFoundException($"Customer with id {dto.CustomerId} not found.");

            var employee = await _employeeRepository.GetByIdAsync(dto.EmployeeId, cancellationToken)
                ?? throw new KeyNotFoundException($"Employee with id {dto.EmployeeId} not found.");

            var treatment = await _treatmentRepository.GetByIdAsync(dto.TreatmentId, cancellationToken)
                ?? throw new KeyNotFoundException($"Treatment with id {dto.TreatmentId} not found.");

            // Create booking (pure domain logic)
            var booking = Booking.Create(
                customer,
                employee,
                treatment,
                dto.BookingDate,
                dto.BookingStartTime
            );

            // Validate internal rules
            var (isValid, error) = booking.ValidateBooking();
            if (!isValid)
                throw new ArgumentException(error);

            // Check for conflicts (domain service)
            if (await _bookingConflictChecker.HasConflictWithAny(booking))
                throw new InvalidOperationException("The booking conflicts with an existing booking.");

            // Save
            await _repository.AddAsync(booking, cancellationToken);
        }

        public async Task UpdateBookingAsync(BookingUpdateDto dto, CancellationToken cancellationToken = default)
        {
            if (dto is null) throw new ArgumentNullException(nameof(dto));

            var booking = await _repository.LoadAsync(dto.Id, cancellationToken)
                ?? throw new KeyNotFoundException($"Booking with id {dto.Id} not found.");

            var customer = await _customerRepository.GetByIdAsync(dto.CustomerId, cancellationToken)
                ?? throw new KeyNotFoundException($"Customer with id {dto.CustomerId} not found.");

            var employee = await _employeeRepository.GetByIdAsync(dto.EmployeeId, cancellationToken)
                ?? throw new KeyNotFoundException($"Employee with id {dto.EmployeeId} not found.");

            var treatment = await _treatmentRepository.GetByIdAsync(dto.TreatmentId, cancellationToken)
                ?? throw new KeyNotFoundException($"Treatment with id {dto.TreatmentId} not found.");

            booking.Update(
                customer,
                employee,
                treatment,
                dto.BookingDate,
                dto.BookingStartTime
            );

            var (isValid, error) = booking.ValidateBooking();
            if (!isValid)
                throw new ArgumentException(error);

            if (await _bookingConflictChecker.HasConflictWithUpdated(booking, booking.Id))
                throw new InvalidOperationException("The booking conflicts with an existing booking.");

            await _repository.UpdateAsync(booking, cancellationToken);
        }

        public async Task DeleteBookingAsync(BookingDeleteDto dto, CancellationToken cancellationToken = default)
        {
            if (dto is null) throw new ArgumentNullException(nameof(dto));

            // Load existing booking with tracked entities from repository
            var booking = await _repository.LoadAsync(dto.Id, cancellationToken)
                ?? throw new KeyNotFoundException($"Booking with id {dto.Id} not found.");

            // Sletter den eksisterende booking
            await _repository.DeleteAsync(booking.Id, cancellationToken);
        }
    }
}
