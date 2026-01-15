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

        public BookingService(
            IBookingRepository repository,
            IBookingPriceService bookingPriceService,
            ICustomerRepository customerRepository,
            IEmployeeRepository employeeRepository,
            ITreatmentRepository treatmentRepository,
            IBookingQuery bookingQuery)
        {
            _repository = repository;
            _bookingPriceService = bookingPriceService;
            _customerRepository = customerRepository;
            _employeeRepository = employeeRepository;
            _bookingQuery = bookingQuery;
            _treatmentRepository = treatmentRepository;
        }


        // Ryk. Til domain helst
        public async Task<bool> CanCreateBookingAsync(Booking newBooking, CancellationToken cancellationToken = default)
        {
            if (newBooking is null) throw new ArgumentNullException(nameof(newBooking));
            if (newBooking.BookingDuration <= TimeSpan.Zero) return false;
            if (newBooking.Employee is null || newBooking.Employee.Id <= 0)
                throw new ArgumentException("Booking must reference a valid employee.", nameof(newBooking));

            var relevant = await _bookingQuery.GetByEmployeeIdAndDateAsync(newBooking.Employee.Id, newBooking.BookingDate, cancellationToken);
            return !HasBookingConflict(newBooking, relevant);
        }

        // Command
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

        // Domain service
        public bool HasBookingConflict(Booking newBooking, IEnumerable<Booking> existingBookings)
        {
            // .Any looper over hele listens og sammenligner.
            return existingBookings.Any(b => newBooking.ConflictsWith(b));
        }

        // ??? discount?
        public IDiscountStrategy? GetDiscountStrategyForCustomerTotalBookings(Customer customer)
        {
            if (customer is null) return null;

            int count = customer.Bookings?.Count ?? 0;

            if (count >= 20)
                return new GoldDiscount();
            if (count >= 10)
                return new SilverDiscount();
            if (count >= 5)
                return new BronzeDiscount();

            return null;
        }

        // ?? booking?
        public decimal CalculatePrice(Booking booking, Employee? employee, Treatment? treatment, Customer? customer = null)
        {
            return _bookingPriceService.CalculateFinalPrice(
                booking,
                employee,
                treatment,
                customer,
                false,
                null,
                null,
                null,
                null,
                null);
        }

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
            if (!await CanCreateBookingAsync(booking, cancellationToken))
                throw new InvalidOperationException("Cannot add booking: time conflict with existing booking.");

            // Persist
            await _repository.AddAsync(booking, cancellationToken);
        }

    }
}
