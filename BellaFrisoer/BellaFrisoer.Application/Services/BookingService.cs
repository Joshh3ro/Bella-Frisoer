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




        /// <summary>
        /// VIRKER IKKE I NUVÆRENDE VERSION
        /// Update funktion for en eksisterende booking. Indlæser den eksisterende booking fra repository,
        /// opdaterer dens egenskaber baseret på de angivne data,
        /// validerer derefter bookingreglerne og kontrollerer for konflikter,
        /// før den gemmer de opdaterede oplysninger.
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="KeyNotFoundException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task UpdateBookingAsync(BookingUpdateDto dto, CancellationToken cancellationToken = default)
        {
            if (dto is null) throw new ArgumentNullException(nameof(dto));

            // Load 
            var booking = await _repository.LoadAsync(dto.Id, cancellationToken)
                ?? throw new KeyNotFoundException($"Booking with id {dto.Id} not found.");

            // Load Relaterede entiteter fra repository
            var customer = await _customerRepository.GetByIdAsync(dto.CustomerId, cancellationToken)
                ?? throw new KeyNotFoundException($"Customer with id {dto.CustomerId} not found.");

            var employee = await _employeeRepository.GetByIdAsync(dto.EmployeeId, cancellationToken)
                ?? throw new KeyNotFoundException($"Employee with id {dto.EmployeeId} not found.");

            var treatment = await _treatmentRepository.GetByIdAsync(dto.TreatmentId, cancellationToken)
                ?? throw new KeyNotFoundException($"Treatment with id {dto.TreatmentId} not found.");

            // Validerer og opdaterer booking via factory method
            booking = Booking.Update(customer, employee, treatment, dto.BookingDate, dto.BookingStartTime);

            if (await _bookingConflictChecker.HasConflictWithUpdated(booking, dto.Id))
                throw new InvalidOperationException("The booking conflicts with an existing booking.");

            // Kalder opdateringsmetode i repository
            await _repository.UpdateAsync(booking, cancellationToken);
        }

        public async Task<IReadOnlyList<Booking>> GetAllAsync(CancellationToken cancellationToken = default)
            => await _bookingQuery.GetAllAsync(cancellationToken);

        public async Task<Booking?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
            => await _bookingQuery.GetByIdAsync(id, cancellationToken);

        public async Task<List<Booking>> FilterBookingsAsync(string searchTerm, CancellationToken cancellationToken = default)
            => await _bookingQuery.FilterBookingsAsync(searchTerm, cancellationToken);

        // Command
        /// <summary>
        /// Sletter en eksisterende booking. Indlæser den eksisterende booking med sporede enheder fra repository, og udfører derefter sletningen.
        /// </summary>
        /// <param name="booking"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task DeleteBookingAsync(BookingDeleteDto dto, CancellationToken cancellationToken = default)
        {
            if (dto is null) throw new ArgumentNullException(nameof(dto));

            // Load existing booking with tracked entities from repository
            var booking = await _repository.LoadAsync(dto.Id, cancellationToken)
                ?? throw new KeyNotFoundException($"Booking with id {dto.Id} not found.");

            // Sletter den eksisterende booking
            await _repository.DeleteAsync(booking.Id, cancellationToken);
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


        /// <summary>
        /// ATilføjer en ny booking baseret på de angivne bookingdetaljer. Validere bookingregler og kontrollerer for konflikter, før den gemmer den nye booking.
        /// </summary>
        /// <param name="dto">The booking details to create the new booking.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the booking details are null.</exception>
        /// <exception cref="KeyNotFoundException">Thrown when the customer, employee, or treatment specified in the booking details cannot be found.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the booking violates domain rules or conflicts with an existing booking.</exception>
        public async Task AddBookingAsync(BookingCreateDto dto, CancellationToken cancellationToken = default)
        {
            if (dto is null) throw new ArgumentNullException(nameof(dto));

            // Loader relaterede entiteter fra repository
            var customer = await _customerRepository.GetByIdAsync(dto.CustomerId, cancellationToken)
                ?? throw new KeyNotFoundException($"Customer with id {dto.CustomerId} not found.");

            var employee = await _employeeRepository.GetByIdAsync(dto.EmployeeId, cancellationToken)
                ?? throw new KeyNotFoundException($"Employee with id {dto.EmployeeId} not found.");

            var treatment = await _treatmentRepository.GetByIdAsync(dto.TreatmentId, cancellationToken)
                ?? throw new KeyNotFoundException($"Treatment with id {dto.TreatmentId} not found.");

            // opretter ny booking via factory methode. valider andre regler i factory method
            var booking = Booking.Create(customer, employee, treatment, dto.BookingDate, dto.BookingStartTime);

            if (await _bookingConflictChecker.HasConflictWithAny(booking))
                throw new InvalidOperationException("The booking conflicts with an existing booking.");

            // Tilføjer ny booking til med repository
            await _repository.AddAsync(booking, cancellationToken);
        }

    }
}
