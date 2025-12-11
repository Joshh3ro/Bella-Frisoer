using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BellaFrisoer.Domain.Models;
using BellaFrisoer.Domain.Models.Discounts;

namespace BellaFrisoer.Application.Interfaces
{
    public interface IBookingService
    {
        Task<bool> CanCreateBookingAsync(Booking newBooking, CancellationToken cancellationToken = default);
        Task AddBookingAsync(Booking booking, CancellationToken cancellationToken = default);
        Task UpdateBookingAsync(Booking booking, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Booking>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Booking?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task DeleteBookingAsync(Booking booking, CancellationToken cancellationToken = default);
        Task<List<Booking>> FilterBookingsAsync(string searchTerm);
        // Returns base price without any discounts applied
        decimal CalculatePrice(Booking booking, Employee? employee, Treatment? treatment, Customer? customer = null);
        void UpdateDurationFromTreatment(Booking booking, Treatment? treatment);

        (bool IsValid, string? ErrorMessage) ValidateBooking(Booking booking);

        // Automatic tier discount based on how many times the customer has booked in total (all treatments).
        // Thresholds: 5+ = Bronze, 10+ = Silver, 20+ = Gold. Returns null if no discount applies.
        IDiscountStrategy? GetDiscountStrategyForCustomerTotalBookings(Customer customer);

        // Legacy/alternative: Automatic tier discount based on how many times the customer has booked the SAME treatment.
        // Thresholds: 5+ = Bronze, 10+ = Silver, 20+ = Gold. Returns null if no discount applies.
        IDiscountStrategy? GetDiscountStrategyForCustomerAndTreatment(Customer customer, int treatmentId);
    }
}