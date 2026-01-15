using BellaFrisoer.Application.Interfaces;
using BellaFrisoer.Domain.Models;
using BellaFrisoer.Domain.Models.Discounts;
using BellaFrisoer.Application.Contracts.Commands;
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




        //Command
        public async Task UpdateBookingAsync(Booking booking, CancellationToken cancellationToken = default)
        {
            if (booking is null) throw new ArgumentNullException(nameof(booking));
            await _repository.UpdateAsync(booking, cancellationToken);
        }

        // Command
        public async Task DeleteBookingAsync(Booking booking, CancellationToken cancellationToken = default)
        {
            await _repository.DeleteAsync(booking.Id, cancellationToken);
        }

        // Query??
        public async Task<IReadOnlyList<Booking>> GetAllAsync(CancellationToken cancellationToken = default)
            => await _bookingQuery.GetAllAsync(cancellationToken);

        // Query
        public async Task<Booking?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
            => await _bookingQuery.GetByIdAsync(id, cancellationToken);

        // Query
        public async Task<List<Booking>> FilterBookingsAsync(string searchTerm, CancellationToken cancellationToken = default)
            => await _bookingQuery.FilterBookingsAsync(searchTerm, cancellationToken);

        // ??? discount?


        // ?? booking?


        // Ai  lavet men godt eksempel på rigtig implmentering
        public async Task AddBookingAsync(BookingCreateDto dto, CancellationToken cancellationToken = default)
        {
            if (dto is null) throw new ArgumentNullException(nameof(dto));

            // Load related entities from repositories (these will be tracked by the same DbContext inside repo implementations)
            var customer = await _customerRepository.GetByIdAsync(dto.CustomerId, cancellationToken)
                ?? throw new KeyNotFoundException($"Customer with id {dto.CustomerId} not found.");

            var employee = await _employeeRepository.GetByIdAsync(dto.EmployeeId, cancellationToken)
                ?? throw new KeyNotFoundException($"Employee with id {dto.EmployeeId} not found.");

            var treatment = await _treatmentRepository.GetByIdAsync(dto.TreatmentId, cancellationToken)
                ?? throw new KeyNotFoundException($"Treatment with id {dto.TreatmentId} not found.");

            // Create domain Booking using tracked entities
            var booking = Booking.Create(customer, employee, treatment, dto.BookingDate, dto.BookingStartTime);

            // Set base price if UI supplied it (the domain already computes BasePrice in constructor,
            // but if you want to preserve UI-calculated price, you could set via reflection or domain API.
            // We'll trust domain calculation here.)

            // Validate booking domain rules
            var (isValid, validationError) = booking.ValidateBooking(booking);
            if (!isValid)
                throw new InvalidOperationException(validationError);

            // Check conflicts
            if (await _bookingConflictChecker.HasConflictWithAny(booking))
                throw new InvalidOperationException("The booking conflicts with an existing booking.");
            // Persist
            await _repository.AddAsync(booking, cancellationToken);
        }

    }
}
