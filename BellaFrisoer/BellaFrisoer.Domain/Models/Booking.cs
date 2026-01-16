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
        public DateTime BookingDateTime => CombineDateTime(BookingDate, BookingStartTime);
        public DateTime BookingEndTime => BookingDateTime.Add(BookingDuration);

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
            Customer = customer ?? throw new ArgumentNullException(nameof(customer));
            Employee = employee ?? throw new ArgumentNullException(nameof(employee));
            Treatment = treatment ?? throw new ArgumentNullException(nameof(treatment));

            if (duration <= TimeSpan.Zero)
                throw new ArgumentException("Booking duration must be greater than zero.", nameof(duration));

            BookingDate = bookingDate.Date;
            BookingStartTime = startTime;
            BookingDuration = duration;

            BasePrice = CalculateBasePrice();

            // Validerer bookingen efter den er oprettet. Ved den køres her, køres den også i booking create og update factory metoderne
            ValidateBooking(this);
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

        public static Booking Update(
            Booking existingBooking,
            Customer customer,
            Employee employee,
            Treatment treatment,
            DateTime bookingDate,
            TimeOnly startTime,
            TimeSpan? duration = null)
        {
            if (existingBooking == null)
                throw new ArgumentNullException(nameof(existingBooking));
            var effectiveDuration = duration ?? TimeSpan.FromMinutes(treatment.Duration);
            return new Booking(customer, employee, treatment, bookingDate, startTime, effectiveDuration)
            {
                Id = existingBooking.Id // Preserver eksisterende ID
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
            // Validerer booking før
            ValidateBooking(booking);
            
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

        /// <summary>
        /// Validerer booking domain regler. Kaster BookingValidationException hvis noget er ugyldigt.
        /// </summary>
        /// <param name="booking">Booking der skal valideres</param>
        public static void ValidateBooking(Booking booking)
        {
            if (booking is null)
                throw new ArgumentNullException(nameof(booking), "Booking mangler...");
            
            if (booking.Customer.Id <= 0)
                throw new ArgumentException("Vælg kunde...", nameof(booking));
            
            if (booking.Employee.Id <= 0)
                throw new ArgumentException("Vælg ansat...", nameof(booking));
            
            if (booking.Treatment.Id <= 0)
                throw new ArgumentException("Vælg behandling...", nameof(booking));
            
            if (booking.BookingStartTime == default)
                throw new ArgumentException("Ugyldigt starttidspunkt...", nameof(booking));
            
            if (booking.BookingDuration == default || booking.BookingDuration <= TimeSpan.Zero)
                throw new ArgumentException("Ugyldig varighed...", nameof(booking));
        }

        #endregion
    }
}
