using System;
using System.ComponentModel.DataAnnotations;

namespace BellaFrisoer.Domain.Models
{
    public class Booking
    {
        public int Id { get; protected set; }
        [Required]
        public DateTime BookingDate { get; protected set; }

        public TimeOnly BookingStartTime { get; protected set; }

        public TimeSpan BookingDuration { get; protected set; }

        public decimal BasePrice { get; protected set; }

        public decimal TotalPrice { get; protected set; }

        public Customer Customer { get; protected set; }

        public Employee Employee { get; protected set; }

        public Treatment Treatment { get; protected set; }

        // Beregnede properties
        private DateTime BookingDateTime => CombineDateTime(BookingDate, BookingStartTime);
        private DateTime BookingEndTime => BookingDateTime.Add(BookingDuration);

        // EF Core kræver parameterløs constructor
        private Booking() { }

        // Privat constructor med alle forretningsregler
        private Booking(
            Customer customer, 
            Employee employee, 
            Treatment treatment,
            DateTime bookingDate, 
            TimeOnly startTime, 
            TimeSpan duration)
        {
            ValidateBooking();

            if (duration <= TimeSpan.Zero)
                throw new ArgumentException("Booking duration must be greater than zero.", nameof(duration));

            BookingDate = bookingDate.Date;
            BookingStartTime = startTime;
            BookingDuration = duration;

            BasePrice = CalculateBasePrice();
        }

        public static Booking Create(
            Customer customer, 
            Employee employee, 
            Treatment treatment,
            DateTime bookingDate, 
            TimeOnly startTime, 
            TimeSpan? duration = null)
        {
            var effectiveDuration = duration ?? TimeSpan.FromMinutes(treatment.Duration);

            return new Booking(customer, employee, treatment, bookingDate, startTime, effectiveDuration);
        }

        public Booking Update(
            //Booking existingBooking,
            Customer customer,
            Employee employee,
            Treatment treatment,
            DateTime bookingDate,
            TimeOnly startTime,
            TimeSpan? duration = null)
        {
            var effectiveDuration = duration ?? TimeSpan.FromMinutes(treatment.Duration);
            return new Booking(customer, employee, treatment, bookingDate, startTime, effectiveDuration)
            {
                Id = this.Id // Preserver eksisterende ID
            };
        }

        public decimal CalculateBasePrice()
        {
            decimal price = 0m;

            if (Treatment.Price > 0)
                price += Treatment.Price;

            if (Employee.HourlyPrice > 0 && BookingDuration > TimeSpan.Zero)
            {
                var minutes = (decimal)BookingDuration.TotalMinutes;
                price += (Employee.HourlyPrice / 60m) * minutes;
            }

            return price;
        }

        public static DateTime CombineDateTime(DateTime date, TimeOnly time)
        {
            return date.Date.Add(time.ToTimeSpan());
        }

        #region CRUD

        public void UpdateDurationFromTreatment(Booking booking)
        {
            var(isValid, errorMessage) = ValidateBooking();
            if (!isValid)
            {
                throw new ArgumentException(errorMessage);
            }
            BookingDuration = TimeSpan.FromMinutes(booking.Treatment.Duration);
            BasePrice = CalculateBasePrice();
        }

        #endregion


        #region Conflicts

        public bool ConflictsWith(Booking other)
        {

            if (other == null) throw new ArgumentNullException(nameof(other));
            if (this.Employee.Id != other.Employee.Id) return false; // forskellig ansat
            if (this.BookingDuration <= TimeSpan.Zero || other.BookingDuration <= TimeSpan.Zero) return false;

            return this.BookingDateTime < other.BookingEndTime && other.BookingDateTime < this.BookingEndTime;
        }


        public (bool IsValid, string? ErrorMessage) ValidateBooking()
        {
            if (Customer.Id <= 0)
                return (false, "Vælg kunde...");
            if (Employee.Id <= 0)
                return (false, "Vælg ansat...");
            if (Treatment.Id <= 0)
                return (false, "Vælg behandling...");
            if (BookingStartTime == default)
                return (false, "Ugyldigt starttidspunkt.");
            if (BookingDuration == default || BookingDuration <= TimeSpan.Zero)
                return (false, "Ugyldig varighed.");
            return (true, null);
        }


        #endregion

    }
}
